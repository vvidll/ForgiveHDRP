using System.Collections;
using _Project.Scripts.DialogueSystem.DialogueWithSalerScripts;
using _Project.Scripts.InventoryScripts;
using _Project.Scripts.MissionsScripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.InteractScripts
{
    // скрипт отвечающий за управление взаимодействием с предметами с помощью луча (Ray)
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] float interactionDistance = 1f;
        [SerializeField] Camera mainCamera;

        public Image interactionUI;

        [SerializeField] LayerMask interactionMask;

        [HideInInspector]
        public bool hitSomething = false;
        [HideInInspector]
        public bool isShowDescription = false;

        float interval = .01f;

        Coroutine checkCoroutine;

        [Header("References From Other Classes")]
        [SerializeField] DialogueWithSaler dialogueWithSaler;
        [SerializeField] TriggerTeleportCarNextLocation triggerTeleportCarNextLocation;
        [SerializeField] BuildingManager buildingManagerBonfire;
        [SerializeField] BuildingManager buildingManagerSleepingBag;
        [SerializeField] ShowMissionsManager showMissionsManager;

        private void Start()
        {
            if(checkCoroutine == null)
                checkCoroutine = StartCoroutine(CheckRaycastHitCoroutine());
        }

        IEnumerator CheckRaycastHitCoroutine()
        {
            while (true)
            {
                InteractionRay();
                yield return new WaitForSeconds(interval);
            }
        }

        /*private void Update()
        {
            InteractionRay();
        }*/

        public void InteractionRay()
        {
            Ray ray = mainCamera.ViewportPointToRay(Vector3.one / 2f);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);

            if (Physics.Raycast(ray, out hit, interactionDistance, interactionMask))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    hitSomething = true;
                    interactable.Description();

                    if (Input.GetKeyDown(KeyCode.F) && dialogueWithSaler.isStartDialogue == false)
                    {
                        Debug.Log("Key F was pressed");
                        interactable.Interact();

                        // iteration items in notepad for check complete mission
                        CounterAddItemsController.CounterAddSurvivalItems(hit);
                        CounterAddItemsController.CounterAddRepairCarItems(hit);
                    }
                }

                if (hit.collider.TryGetComponent(out IInteractableBuildings interactablePlacedObjects))
                {
                    if(buildingManagerBonfire.isHasPlacedItem == true)
                        InteractPlacedObjects(interactablePlacedObjects, hit);

                    // синхронизоровать взаимодействие раставленных объектов с миссиями
                    if (buildingManagerSleepingBag.isHasPlacedItem == true)
                        InteractPlacedObjects(interactablePlacedObjects, hit);
                }
            }

            else
                hitSomething = false;

            interactionUI.gameObject.SetActive(hitSomething);
        }

        private void OnDestroy()
        {
            if(checkCoroutine != null)
                StopCoroutine(checkCoroutine);
        }

        void InteractPlacedObjects(IInteractableBuildings interactablePlacedObjects, RaycastHit hitInfo) 
        {
            hitSomething = true;
            isShowDescription = true;

            if (hitSomething == true && isShowDescription == true)
                interactablePlacedObjects.DescriptionBuildings();

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (hitInfo.collider.name == "Firewood") 
                {
                    showMissionsManager.ShowMissionPlaceTent();
                }

                Debug.Log("Key F was pressed");
                interactablePlacedObjects.InteractBuildings();

                hitSomething = false;
                isShowDescription = false;
            }
        }
    }
}

