using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.CheckpointsScripts
{
    // скрипт отвечающий за общую работу всех чекпоинтов
    public class CheckpointManager : MonoBehaviour
    {
        public GameObject[] activeCheckpointsArray;

        int countTriggerCheckpoint = -1; // т.к массив начинается с 0

        public const string CountTriggerCheckpointKey = "count_trigger_checkpoint";

        private void Awake()
        {
            // this code worked after load control point
            if (PlayerPrefs.HasKey(CheckpointController.CheckpointActiveKey) && 
                PlayerPrefs.HasKey(CountTriggerCheckpointKey) && SceneManager.GetActiveScene().name == "GameScene")
            {
                CheckpointController.checkpointActive = PlayerPrefs.GetInt(CheckpointController.CheckpointActiveKey);
                countTriggerCheckpoint = PlayerPrefs.GetInt(CountTriggerCheckpointKey);

                if (CheckpointController.checkpointActive == 0 && countTriggerCheckpoint != -1) 
                {
                    for (int i = 0; i <= countTriggerCheckpoint && i < activeCheckpointsArray.Length; i++) 
                    {
                        activeCheckpointsArray[i].gameObject.SetActive(false);
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