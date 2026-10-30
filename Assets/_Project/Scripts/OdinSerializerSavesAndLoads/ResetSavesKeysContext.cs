using _Project.Scripts.CheckpointsScripts;
using UnityEngine;

namespace _Project.Scripts.OdinSerializerSavesAndLoads
{
    public class ResetSavesKeysContext : MonoBehaviour
    {
        private void Awake()
        {
            Debug.LogError("Important! before launch the game, delete all keys for checkpoints everything!\nAnd also in folder saves the game in the directory, you need to delete <<Autosave.log>> files!");
        }

        [ContextMenu("Reset Saves Keys (Польз.)")]
        public void DeleteKeys()
        {
            PlayerPrefs.DeleteKey(CheckpointController.CheckpointActiveKey);
            PlayerPrefs.DeleteKey(CheckpointManager.CountTriggerCheckpointKey);

            Debug.Log("Ключи СОХРАНЕНИЯ чекпоинтов УДАЛЕНЫ");
        }
    }
}
