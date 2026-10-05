using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.CameraScripts
{
    // скрипт отвечающий за сынсу мыши
    public class SensivityController : MonoBehaviour
    {
        public const string SensivitySliderValueKey = "sensivity_slider_value_settings";
        public const string SensivitySettingsKey = "sensivity_settings";

        [SerializeField] public float currentSensivity = 500f;

        float minSensivity = 100f;
        float maxSensivity = 1000f;

        [SerializeField] Slider sliderSensivity;

        private void Awake()
        {
            if (PlayerPrefs.HasKey(SensivitySliderValueKey) && PlayerPrefs.HasKey(SensivitySettingsKey))
            {
                sliderSensivity.value = PlayerPrefs.GetFloat(SensivitySliderValueKey);
                currentSensivity = PlayerPrefs.GetFloat(SensivitySettingsKey);

                //Debug.Log("Загрузка сынсы мыши");
            }
        }
        private void Start()
        {
            sliderSensivity.onValueChanged.AddListener(value =>
            {
                currentSensivity = Mathf.Max(minSensivity, value * maxSensivity);
            });
        }

        private void OnDisable()
        {
            sliderSensivity.onValueChanged.RemoveAllListeners();
            PlayerPrefs.SetFloat(SensivitySliderValueKey, sliderSensivity.value);
            PlayerPrefs.SetFloat(SensivitySettingsKey, currentSensivity);

            //Debug.Log("Сохранение сынсы мыши");
        }

        [ContextMenu("Delete Key Sensivity (Польз.)")]
        public void DeleteKeySensivity() => PlayerPrefs.DeleteKey(SensivitySliderValueKey);
    }
}
