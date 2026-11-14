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

        public int countTriggerCheckpointDisabled = -1; // т.к массив начинается с 0

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
            if (PlayerPrefs.HasKey(CheckpointController.CheckpointEnabledKey) && 
                PlayerPrefs.HasKey(CheckpointController.CheckpointDisabledKey) &&
                PlayerPrefs.HasKey(CountTriggerCheckpointKey) && 
                SceneManager.GetActiveScene().name == "GameScene")
            {
                CheckpointController.checkpointEnabled = PlayerPrefs.GetInt(CheckpointController.CheckpointEnabledKey);
                CheckpointController.checkpointDisabled = PlayerPrefs.GetInt(CheckpointController.CheckpointDisabledKey);
                countTriggerCheckpointDisabled = PlayerPrefs.GetInt(CountTriggerCheckpointKey);

                if (CheckpointController.checkpointEnabled == 1) 
                {
                    for (int i = 0; i < checkpointsArray.Length; i++) 
                    {
                        checkpointsArray[i].gameObject.SetActive(true);
                    }
                }

                if (CheckpointController.checkpointDisabled == 0 && countTriggerCheckpointDisabled != -1)
                {
                    for (int i = 0; i <= countTriggerCheckpointDisabled && i < checkpointsArray.Length; i++)
                    {
                        checkpointsArray[i].gameObject.SetActive(false);

                        Debug.Log("counts disabled checkpoints: " + countTriggerCheckpointDisabled);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Checkpoint")) 
            {
                countTriggerCheckpointDisabled++;
                PlayerPrefs.SetInt(CountTriggerCheckpointKey, countTriggerCheckpointDisabled);
            }

        }
    }
}