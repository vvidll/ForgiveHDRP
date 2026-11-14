using System;
using System.Collections.Generic;
using System.IO;
using _Project.Scripts.CarScripts;
using _Project.Scripts.DialogueSystem.DialogueWithSalerScripts;
using _Project.Scripts.InteractScripts;
using _Project.Scripts.InteractScripts.CarInteractScripts;
using _Project.Scripts.InventoryScripts;
using _Project.Scripts.PlayerScripts;
using _Project.Scripts.ProjectSettingsScripts;
using OdinSerializer;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;


namespace _Project.Scripts.OdinSerializerSavesAndLoads
{
    public class OdinSerializerControllerSaveAndLoad : MonoBehaviour
    {
        public const string parentNameFirst = "--DINAMIC_OBJECTS--";
        public const string parentNameSecond = "Fairheaven_LT80 (Car)";

        public const string childName = "Player";


        public const string CountFileKey = "count_file";

        readonly string filePath = "C:/Users/Impossible/AppData/LocalLow/Noise Wind/NewForgive/_Autosave.log";

        [Header("Position and Rotation Car")]
        [SerializeField] Transform carTransform;

        [Header("Settings Initial Animation")]
        [SerializeField] SettingsAnimationCarAndPlayerPositionStartGame settingsInitialAnimation;

        [Header("RigBuilder")]
        [SerializeField] RigBuilder rigBuilder;

        [Header("References From Other Classes")]
        [SerializeField] DrivingPlayer drivingPlayer;
        [SerializeField] InteractionDoorCar interactionDoorCar;
        [SerializeField] DialogueWithSaler dialogueWithSaler;
        [SerializeField] TeleportCar teleportCar;
        [SerializeField] CheckCompleteTasksNotepad checkCompleteTasksNotepad;
        [SerializeField] CommonInteractionItems[] commonEnabledInteractionItems; // array enabled items

        /*public GameObject[] itemInteraction
        {
            get { return this.saveDataPlayer.itemInteract; }
            set { this.saveDataPlayer.itemInteract = value; }
        }*/


        /*public GameObject[] toggleIsOnInNotepad
        {
            get { return this.saveDataPlayer.toggleIsOnInNotepad; }
            set { this.saveDataPlayer.toggleIsOnInNotepad = value; }
        }*/

        private void Awake()
        {
            if (File.Exists(filePath) && SceneManager.GetActiveScene().name == "GameScene") 
                LoadData();
        }

        private void Start()
        {
            //Debug.Log(Application.persistentDataPath);
        }

        public void SaveData(SaveDataPlayer saveDataPlayer)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    CommonSaveData(saveDataPlayer);

                    byte[] bytes = SerializationUtility.SerializeValue(saveDataPlayer, DataFormat.JSON);

                    fileStream.Write(bytes);

                    Debug.Log("Saved Data for Autosave");
                }
            }

            catch (IOException ex)
            {
                Debug.LogError(ex.Message);
            }

            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        public void LoadData()
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    byte[] bytes = new byte[fileStream.Length]; // создаем массив размерностью - размера файла
                    fileStream.Read(bytes, 0, bytes.Length); // читаем все до последнего байта

                    SaveDataPlayer saveDataPlayer = SerializationUtility.DeserializeValue<SaveDataPlayer>(bytes, DataFormat.JSON);

                    CommonLoadData(saveDataPlayer);

                    Debug.Log("Load Data from Autosave");
                }
            }

            catch (IOException ex)
            {
                Debug.LogError(ex.Message);
            }

            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }

        }

        public void CommonLoadData(SaveDataPlayer saveDataPlayer)
        {
            settingsInitialAnimation.isParking = saveDataPlayer.isParking;

            settingsInitialAnimation.carAnimator.enabled = saveDataPlayer.isAnimatorCar;

            settingsInitialAnimation.isFirstLaunchGame = saveDataPlayer.isFirtsLaunchGame;
            drivingPlayer.isInCar = saveDataPlayer.isInCar;
            settingsInitialAnimation.isEnoughPlayInitialAnimation = saveDataPlayer.isEnoughPlayInitialAnimation;

            LoadParent(saveDataPlayer);

            this.transform.position = saveDataPlayer.playerPosition;
            this.transform.rotation = saveDataPlayer.playerRotation;

            dialogueWithSaler.hasBoughtItemsInShop = saveDataPlayer.hasBoughtItemsInShop;

            checkCompleteTasksNotepad.completeMissionItemsForSurvival = saveDataPlayer.completeMissionItemsForSurvival;

            if (dialogueWithSaler.hasBoughtItemsInShop == true && checkCompleteTasksNotepad.completeMissionItemsForSurvival == true)
            {
                foreach (CommonInteractionItems interactionItems in commonEnabledInteractionItems)
                    interactionItems.gameObject.SetActive(false);
            }    

            teleportCar.isSecondLocation = saveDataPlayer.isSecondLocation;

            Debug.Log($"Position: {this.transform.position}\nRotation: {this.transform.rotation}");

            if (settingsInitialAnimation.isParking == true)
            {
                settingsInitialAnimation.positionCarAfterParking = saveDataPlayer.carPosition;
                settingsInitialAnimation.rotationCarAfterParking = saveDataPlayer.carRotation;
            }

            // idk working it or not
            carTransform.position = saveDataPlayer.carPosition;
            carTransform.rotation = saveDataPlayer.carRotation;

            for (int i = 1; i < rigBuilder.layers.Count; i++)
            {
                rigBuilder.layers[i].active = saveDataPlayer.isLayersRigBuilderPlayer;
            }

            //this.itemInteraction = this.saveDataPlayer.itemInteract;

            //this.toggleIsOnInNotepad = this.saveDataPlayer.toggleIsOnInNotepad;
        }

        public void CommonSaveData(SaveDataPlayer saveDataPlayer) 
        {
            settingsInitialAnimation.OffInitialAnimation();

            saveDataPlayer.isParking = settingsInitialAnimation.isParking;

            // for positionCarAfterParking and rotationCarAfterParking
            saveDataPlayer.isAnimatorCar = settingsInitialAnimation.carAnimator.enabled;

            saveDataPlayer.playerPosition = this.transform.position;
            saveDataPlayer.playerRotation = this.transform.rotation;

            // for player(child object) set under Empty Object(parent) 
            if (drivingPlayer.isParentDinamicObjects == true)
                SaveParent(drivingPlayer.isParentDinamicObjects, saveDataPlayer);

            saveDataPlayer.hasBoughtItemsInShop = dialogueWithSaler.hasBoughtItemsInShop;

            saveDataPlayer.completeMissionItemsForSurvival = checkCompleteTasksNotepad.completeMissionItemsForSurvival;

            saveDataPlayer.isSecondLocation = teleportCar.isSecondLocation;

            // !!! впринципе можно не сохранять, то что игрок находится под машиной как дочерний объект, т.к это только все ломает,
            // лучше автоматически сохранять покупки (в первой локе) и сохранять выход из машины (во второй локе)
            // надо проверять в какой локе находится игрок !!!

            // for player(child object) set under Car(parent)
            //if (interactionDoorCar.isParentCar == false)
              //  SaveParent(interactionDoorCar.isParentCar, saveDataPlayer);

            Debug.Log($"Position: {saveDataPlayer.playerPosition}\nRotation: {saveDataPlayer.playerRotation}");

            // after parking
            if (settingsInitialAnimation.isParking == true)
            {
                saveDataPlayer.carPosition = settingsInitialAnimation.positionCarAfterParking;
                saveDataPlayer.carRotation = settingsInitialAnimation.rotationCarAfterParking;
            }

            // idk working it or not
            saveDataPlayer.carPosition = carTransform.position;
            saveDataPlayer.carRotation = carTransform.rotation;

            // need save current animation clip for car
            saveDataPlayer.isFirtsLaunchGame = settingsInitialAnimation.isFirstLaunchGame;
            saveDataPlayer.isInCar = drivingPlayer.isInCar;
            saveDataPlayer.isEnoughPlayInitialAnimation = settingsInitialAnimation.isEnoughPlayInitialAnimation;

            // need save current animation clip for player
            for (int i = 1; i < rigBuilder.layers.Count; i++)
            {
                saveDataPlayer.isLayersRigBuilderPlayer = rigBuilder.layers[i].active;
            }

            //this.saveDataPlayer.itemInteract = this.itemInteraction;

            //this.saveDataPlayer.toggleIsOnInNotepad = this.toggleIsOnInNotepad;
        }

        /// <summary>
        /// true = "DrivingPlayer" and false = "InteractionDoorCar"
        /// </summary>
        public void SaveParent(bool parent, SaveDataPlayer saveDataPlayer) 
        {
            Transform target = parent ? drivingPlayer.parentObject : interactionDoorCar.carParentObject;

            this.transform.SetParent(target);

            saveDataPlayer.childId = childName;

            saveDataPlayer.parentId = parent ? parentNameFirst : parentNameSecond;

            Debug.Log(target);
        }

        /// <summary>
        /// true = "DrivingPlayer" and false = "InteractionDoorCar"
        /// </summary>
        public void LoadParent(SaveDataPlayer saveDataPlayer) 
        {
            Transform targetParent = saveDataPlayer.parentId == parentNameFirst ? drivingPlayer.parentObject : interactionDoorCar.carParentObject;

            this.transform.SetParent(targetParent);

            Debug.Log(targetParent);
        }
    }

}

[Serializable]
public class SaveDataPlayer
{
    public string childId;
    public string parentId;

    [Header("Position and Rotation Player")]
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    [Header("Position and Rotation Car")]
    public Vector3 carPosition;
    public Quaternion carRotation;

    public bool isFirtsLaunchGame;
    public bool isParking;

    public bool isAnimatorCar;

    public bool isLayersRigBuilderPlayer;

    public bool isEnoughPlayInitialAnimation;

    public bool isInCar;

    public bool hasBoughtItemsInShop;

    public bool isSecondLocation;

    public bool completeMissionItemsForSurvival;

    //public GameObject[] itemInteract;
    //public GameObject[] toggleIsOnInNotepad;
}