using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.CheckpointsScripts
{
    /*public static class Checkpoints
    {
        public static Dictionary<int, string> CkeckpointsInfo;
    }*/

    // скрипт отвечающий за общую работу всех чекпоинтов
    public class CheckpointManager : MonoBehaviour
    {
        public GameObject[] checkpointsArray;

        int countTriggerCheckpoint = -1; // т.к массив начинается с 0

        public const string CountTriggerCheckpointKey = "count_trigger_checkpoint";

        private void Awake()
        {
            // checks number and name checkpoint before launch the game
            /*Checkpoints.CkeckpointsInfo = new Dictionary<int, string>()
            {
                { 0, "Checkpoint (ExitFromCar)" },
                { 1, "Checkpoint (EnterInShop)" },
                { 2, "Checkpoint (SitInTheCar)" },
                { 3, "Checkpoint (FindedSuitableLocation)" }
            };*/
            Debug.LogError("checks number and name checkpoint before launch the game!");

            // this code worked after load control point
            if (PlayerPrefs.HasKey(CheckpointController.CheckpointActiveKey) && 
                PlayerPrefs.HasKey(CountTriggerCheckpointKey) && SceneManager.GetActiveScene().name == "GameScene")
            {
                CheckpointController.checkpointActive = PlayerPrefs.GetInt(CheckpointController.CheckpointActiveKey);
                countTriggerCheckpoint = PlayerPrefs.GetInt(CountTriggerCheckpointKey);

                if (CheckpointController.checkpointActive == 0 && countTriggerCheckpoint != -1) 
                {
                    for (int i = 0; i <= countTriggerCheckpoint && i < checkpointsArray.Length; i++) 
                    {
                        checkpointsArray[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Checkpoint")) 
            {
                countTriggerCheckpoint++;
                PlayerPrefs.SetInt(CountTriggerCheckpointKey, countTriggerCheckpoint);
            }

        }
    }
}