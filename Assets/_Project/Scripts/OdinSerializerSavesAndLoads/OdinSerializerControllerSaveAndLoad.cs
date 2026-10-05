using System;
using System.IO;
using OdinSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace _Project.Scripts.OdinSerializerSavesAndLoads
{
    public class OdinSerializerControllerSaveAndLoad : MonoBehaviour
    {
        public const string CountFileKey = "count_file";

        readonly string filePath = "C:/Users/Impossible/AppData/LocalLow/Noise Wind/NewForgive/_Autosave.log";

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
            this.transform.position = saveDataPlayer.playerPosition; // не загружает игрока на нужную сохраненную позицию
            this.transform.rotation = saveDataPlayer.playerRotation;

            //this.itemInteraction = this.saveDataPlayer.itemInteract;

            //this.toggleIsOnInNotepad = this.saveDataPlayer.toggleIsOnInNotepad;
        }

        public void CommonSaveData(SaveDataPlayer saveDataPlayer) 
        {
            saveDataPlayer.playerPosition = this.transform.position;
            saveDataPlayer.playerRotation = this.transform.rotation;

            //this.saveDataPlayer.itemInteract = this.itemInteraction;

            //this.saveDataPlayer.toggleIsOnInNotepad = this.toggleIsOnInNotepad;
        }
    }

}

[Serializable]
public class SaveDataPlayer
{
    [Header("Position and Rotation")]
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    //public GameObject[] itemInteract;
    //public GameObject[] toggleIsOnInNotepad;
}
