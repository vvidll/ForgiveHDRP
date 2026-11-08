using System;
using System.IO;
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
        public const string CountFileKey = "count_file";

        readonly string filePath = "C:/Users/Impossible/AppData/LocalLow/Noise Wind/NewForgive/_Autosave.log";

        [Header("Position and Rotation Car")]
        [SerializeField] Transform carTransform;

        [Header("Settings Initial Animation")]
        [SerializeField] SettingsAnimationCarAndPlayerPositionStartGame settingsInitialAnimation;

        [Header("RigBuilder")]
        [SerializeField] RigBuilder rigBuilder;

        [SerializeField] DrivingPlayer drivingPlayer;

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
            settingsInitialAnimation.isParking = saveDataPlayer.isParkingAnimationControl;

            settingsInitialAnimation.carAnimator.enabled = saveDataPlayer.isAnimatorCarActivationControl;

            this.transform.position = saveDataPlayer.playerPosition; // не загружает игрока на нужную сохраненную позицию
            this.transform.rotation = saveDataPlayer.playerRotation;

            if (settingsInitialAnimation.isParking == true)
            {
                settingsInitialAnimation.positionCarAfterParking = saveDataPlayer.carPosition;
                settingsInitialAnimation.rotationCarAfterParking = saveDataPlayer.carRotation;
            }

            // idk working it or not
            carTransform.position = saveDataPlayer.carPosition;
            carTransform.rotation = saveDataPlayer.carRotation;

            settingsInitialAnimation.isFirstLaunchGame = saveDataPlayer.isFirtsLaunchGameAnimationControl;
            drivingPlayer.isInCar = saveDataPlayer.isInCarControl;
            settingsInitialAnimation.isEnoughPlayInitialAnimation = saveDataPlayer.isEnoughPlayInitialAnimation;

            for (int i = 1; i < rigBuilder.layers.Count; i++)
            {
                rigBuilder.layers[i].active = saveDataPlayer.isLayersRigBuilderPlayerActive;
            }

            //this.itemInteraction = this.saveDataPlayer.itemInteract;

            //this.toggleIsOnInNotepad = this.saveDataPlayer.toggleIsOnInNotepad;
        }

        public void CommonSaveData(SaveDataPlayer saveDataPlayer) 
        {
            settingsInitialAnimation.OffInitialAnimation();

            saveDataPlayer.isParkingAnimationControl = settingsInitialAnimation.isParking;

            // for positionCarAfterParking and rotationCarAfterParking
            saveDataPlayer.isAnimatorCarActivationControl = settingsInitialAnimation.carAnimator.enabled;

            saveDataPlayer.playerPosition = this.transform.position;
            saveDataPlayer.playerRotation = this.transform.rotation;

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
            saveDataPlayer.isFirtsLaunchGameAnimationControl = settingsInitialAnimation.isFirstLaunchGame;
            saveDataPlayer.isInCarControl = drivingPlayer.isInCar;
            saveDataPlayer.isEnoughPlayInitialAnimation = settingsInitialAnimation.isEnoughPlayInitialAnimation;

            // need save current animation clip for player
            for (int i = 1; i < rigBuilder.layers.Count; i++)
            {
                saveDataPlayer.isLayersRigBuilderPlayerActive = rigBuilder.layers[i].active;
            }

            //this.saveDataPlayer.itemInteract = this.itemInteraction;

            //this.saveDataPlayer.toggleIsOnInNotepad = this.toggleIsOnInNotepad;
        }
    }

}

[Serializable]
public class SaveDataPlayer
{
    [Header("Position and Rotation Player")]
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    [Header("Position and Rotation Car")]
    public Vector3 carPosition;
    public Quaternion carRotation;

    public bool isFirtsLaunchGameAnimationControl;
    public bool isParkingAnimationControl;

    public bool isAnimatorCarActivationControl;

    public bool isLayersRigBuilderPlayerActive;

    public bool isEnoughPlayInitialAnimation;

    public bool isInCarControl;

    //public GameObject[] itemInteract;
    //public GameObject[] toggleIsOnInNotepad;
}
