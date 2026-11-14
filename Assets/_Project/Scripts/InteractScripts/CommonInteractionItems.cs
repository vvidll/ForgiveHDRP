using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.InteractScripts
{
    // скрипт отвечающий за отображение в блокноте предметов подобранных в инвентарь
    // и (метод описания предметов выведен в отдельный метод для удобства)
    public class CommonInteractionItems : MonoBehaviour
    {
        public bool isCurrentEnabledItem = true;

        protected void CommonInteractItem(Toggle toggle)
        {
            toggle.isOn = true;

            isCurrentEnabledItem = false;

            this.gameObject.SetActive(isCurrentEnabledItem);
        }

        protected void CommonDescriptionItem(TMP_Text tmpText, string text) => tmpText.text = text;


    }
}
