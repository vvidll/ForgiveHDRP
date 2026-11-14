using System.Collections.Generic;
using _Project.Scripts.OdinSerializerSavesAndLoads;
using UnityEngine;

namespace _Project.Scripts.CheckpointsScripts
{

    // скрипт отвечающий за отдельный пройденный игроком чекпоинт 
    public class CheckpointController : MonoBehaviour
    {
        SaveDataPlayer saveDataPlayer = new SaveDataPlayer();

        [SerializeField] SaveDataInfo saveDataInfo;

        public const string CheckpointEnabledKey = "enabled_checkpoint";
        public const string CheckpointDisabledKey = "disabled_checkpoint";

        public static int checkpointEnabled = 1, checkpointDisabled = 0; // 1 - enabled, 0 - disabled

        bool isEnterCheckpoint = false;

        private void Start()
        {
            if(checkpointEnabled == 1 && isEnterCheckpoint == false)
                PlayerPrefs.SetInt(CheckpointEnabledKey, checkpointEnabled);
        }

        private void OnTriggerEnter(Collider other)
        {
            // this code off current checkpoint and save data
            if (other.TryGetComponent(out OdinSerializerControllerSaveAndLoad odinSerializerControllerSaveAndLoad))
            {
                isEnterCheckpoint = true;

                odinSerializerControllerSaveAndLoad.SaveData(saveDataPlayer);

                saveDataInfo.InfoActive();

                gameObject.SetActive(false);
                checkpointDisabled = 0;

                PlayerPrefs.SetInt(CheckpointDisabledKey, checkpointDisabled);
            }
        }
    }
}