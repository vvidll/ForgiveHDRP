using _Project.Scripts.OdinSerializerSavesAndLoads;
using UnityEngine;

namespace _Project.Scripts.CheckpointsScripts
{

    // скрипт отвечающий за отдельный пройденный игроком чекпоинт 
    public class CheckpointController : MonoBehaviour
    {
        SaveDataPlayer saveDataPlayer = new SaveDataPlayer();

        [SerializeField] SaveDataInfo saveDataInfo;

        public const string CheckpointActiveKey = "active_checkpoint";

        public static int checkpointActive = 1;

        private void OnTriggerEnter(Collider other)
        {
            // this code off current checkpoint and save data
            if (other.TryGetComponent(out OdinSerializerControllerSaveAndLoad odinSerializerControllerSaveAndLoad))
            {
                odinSerializerControllerSaveAndLoad.SaveData(saveDataPlayer);

                saveDataInfo.InfoActive();

                gameObject.SetActive(false);
                checkpointActive = 0;

                PlayerPrefs.SetInt(CheckpointActiveKey, checkpointActive);
            }
        }
    }
}