using System.Collections;
using _Project.Scripts.InteractScripts.CarInteractScripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

namespace _Project.Scripts.PlayerScripts
{
    public class DrivingPlayer : MonoBehaviour
    {
        public const string HoldKeyF = "hold_key_f";
        public const string RigBuilderListCountKey = "rig_builder_list_count";

        [SerializeField] Animator playerAnimator;
        [SerializeField] Animator carDoorAnimator;
        [SerializeField] AnimationClip playerAnimationClip;

        [SerializeField] Transform playerTransform;

        //[SerializeField] CharacterController characterController;

        public Image imageInteractHold;

        [SerializeField] RigBuilder rigBuilder;

        [SerializeField] SphereCollider sphereColliderLeftDoor;


        [Header("Parent Object")]
        [SerializeField] Transform parentObject;

        [Header("Child Object")]
        [SerializeField] Transform player;

        //[HideInInspector]
        public bool isInCar = false;

        public int isHoldKeyF = 0; // false
        public int rigBuilderCount = 0;

        [Header("References From Other Classes")]
        [SerializeField] InteractionDoorCar interactionDoorCar;

        private void Start()
        {
            imageInteractHold.gameObject.SetActive(false);

            // start the game
            playerAnimator.SetTrigger("isDrivingStartGame");
        }

        private void Update()
        {
            sphereColliderLeftDoor.enabled = !isInCar; // off interaction UI icon

            if (Input.GetKey(KeyCode.F) && imageInteractHold.fillAmount < 1 && imageInteractHold.gameObject.activeSelf == true
            && isHoldKeyF == 0)
            {
                imageInteractHold.fillAmount += 0.4f * Time.deltaTime;

                imageInteractHold.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 6f);


                if (imageInteractHold.fillAmount == 1)
                {
                    imageInteractHold.gameObject.SetActive(false);
                    ExitingCar();

                    isHoldKeyF = 1; // hold key end
                }

            }

            else if (imageInteractHold.fillAmount < 1 && imageInteractHold.gameObject.activeSelf == true)
            {
                imageInteractHold.fillAmount -= 0.2f * Time.deltaTime;

                imageInteractHold.transform.DOScale(new Vector3(1f, 1f, 1f), 6f);
            }
        }

        public void ExitingCar()
        {
            for (int i = 1; i < rigBuilder.layers.Count; i++)
            {
                rigBuilder.layers[i].active = false;
            }

            carDoorAnimator.SetBool("isOpenAndCloseDoor", true);

            playerAnimator.SetTrigger("isExitingCar");

            player.SetParent(parentObject);

            StartCoroutine(EndPlayAnimationExitingCarCoroutine());

        }

        IEnumerator EndPlayAnimationExitingCarCoroutine()
        {
            yield return new WaitForSeconds(7);

            playerAnimator.SetTrigger("isIdle");

            imageInteractHold.fillAmount = 0;

            isInCar = false;

            carDoorAnimator.SetBool("isOpenAndCloseDoor", false);

        }

        [ContextMenu("Reset Key Driver (Польз.)")]
        public void DeleteKeys()
        {
            PlayerPrefs.DeleteKey(HoldKeyF);
            PlayerPrefs.DeleteKey(RigBuilderListCountKey);

            Debug.Log("Удаление ключей Водителя");
        }

    }
}