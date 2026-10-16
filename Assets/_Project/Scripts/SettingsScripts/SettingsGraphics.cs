using System.Collections.Generic;
using _Project.Scripts.FPSScripts;
using TMPro;
using UnityEngine;
using UnityEngine.NVIDIA;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData;

public class SettingsGraphics : MonoBehaviour
{
    [SerializeField] GameObject panelConfirmationWindowForSaves;

    [Header("HDRP Camera")]
    [SerializeField] HDAdditionalCameraData hdAdditionalCameraData;

    [Header("FPS")]
    [SerializeField] Toggle fpsToggle;
    [SerializeField] FPSDisplay fPSDisplay;

    int showFpsToggle; // 1 - active, 0 - nope active
    int showFpsCounter;

    [Header("Dropdown")]
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] TMP_Dropdown displayDropdown;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown dlssDropdown;
    [SerializeField] TMP_Dropdown anisotropicFilteringDropdown;
    [SerializeField] TMP_Dropdown antiAliazingDropdown;

    [Header("Text")]
    [SerializeField] TMP_Text qualityTextLabel;
    [SerializeField] TMP_Text displayTextLabel;
    [SerializeField] TMP_Text resolutionTextLabel;
    [SerializeField] TMP_Text dlssTextLabel;
    [SerializeField] TMP_Text anisotropicFilteringLabel;
    [SerializeField] TMP_Text antiAliazingLabel;

    internal bool isHasEditSettingQuality = false;
    internal bool isHasEditSettingDisplay = false;
    internal bool isHasEditSettingResolution = false;
    internal bool isHasEditSettingDLSS = false;
    internal bool isHasEditSettingsAnisotropicFiltering = false;
    internal bool isHasEditSettingsAntiAliazing = false;

    Resolution[] resolutions;

    private void Awake()
    {
        /*PlayerPrefs.DeleteKey("Quality");
        PlayerPrefs.DeleteKey("Display");
        PlayerPrefs.DeleteKey("Resolutions");
        PlayerPrefs.DeleteKey("DLSS");
        PlayerPrefs.DeleteKey("show_fps_toggle");
        PlayerPrefs.DeleteKey("hide_fps_toggle");
        PlayerPrefs.DeleteKey("show_fps_counter");
        PlayerPrefs.DeleteKey("hide_fps_counter");*/

        resolutionDropdown.ClearOptions();
        List<string> oprions = new List<string>();
        resolutions = Screen.resolutions;
        int currentResulotionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRateRatio + "Hz";
            oprions.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResulotionIndex = i;
        }

        resolutionDropdown.AddOptions(oprions);
        resolutionDropdown.RefreshShownValue();

        if (SceneManager.GetActiveScene().name == "GameScene")
            LoadGraphicsSettings(currentResulotionIndex);

        else if (SceneManager.GetActiveScene().name == "MainMenuSceneDay")
            LoadGraphicsSettings(currentResulotionIndex);
    }

    public void ShowOrHideFPS() 
    {
        switch (SceneManager.GetActiveScene().name) 
        {
            case "GameScene":

                if (fpsToggle.isOn == true)
                    SaveFPSCounterONInScenes();

                else
                    SaveFPSCounterOFFInScenes();

                break;

            case "MainMenuSceneDay":

                if (fpsToggle.isOn == true)
                    SaveFPSCounterONInScenes();

                else
                    SaveFPSCounterOFFInScenes();

                break;
        }
    }

    public void Quality(int index) 
    {
        QualitySettings.SetQualityLevel(index);

        qualityTextLabel.text = "Quality*";
        isHasEditSettingQuality = true;
    } 

    public void Display(int displayIndex)
    {
        FullScreenMode mode = (FullScreenMode)displayIndex;
        Screen.fullScreenMode = mode;

        displayTextLabel.text = "Display*";
        isHasEditSettingDisplay = true;
    }

    public void Resolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        resolutionTextLabel.text = "Resolution*";
        isHasEditSettingResolution = true;
    }

    public void DLSS(int dlssIndex) 
    {
        switch (dlssIndex) 
        {
            case 0:
                hdAdditionalCameraData.allowDeepLearningSuperSampling = false;

                antiAliazingDropdown.interactable = true;
                dlssDropdown.interactable = false;

                dlssTextLabel.text = "DLSS*";
                isHasEditSettingDLSS = true;

                break;

            case 1:
                hdAdditionalCameraData.deepLearningSuperSamplingQuality = (uint)DLSSQuality.MaximumQuality;
                ChangeDLSS();
                break;

            case 2:
                hdAdditionalCameraData.deepLearningSuperSamplingQuality = (uint)DLSSQuality.Balanced;
                ChangeDLSS();
                break;

            case 3:
                hdAdditionalCameraData.deepLearningSuperSamplingQuality = (uint)DLSSQuality.MaximumPerformance;
                ChangeDLSS();
                break;

            case 4:
                hdAdditionalCameraData.deepLearningSuperSamplingQuality = (uint)DLSSQuality.UltraPerformance;
                ChangeDLSS();               
                break;

            case 5:
                hdAdditionalCameraData.deepLearningSuperSamplingQuality = (uint)DLSSQuality.DLAA;
                ChangeDLSS();
                break;
        }
    }

    public void ChangeDLSS() 
    {
        dlssDropdown.interactable = true;
        antiAliazingDropdown.interactable = false;

        hdAdditionalCameraData.antialiasing = AntialiasingMode.None;

        hdAdditionalCameraData.allowDeepLearningSuperSampling = true;

        dlssTextLabel.text = "DLSS*";
        isHasEditSettingDLSS = true;
    }

    public void AnisotropicFiltering(int anisotropicFilteringIndex) 
    {
        AnisotropicFiltering anisotropicFiltering = (AnisotropicFiltering)anisotropicFilteringIndex;
        QualitySettings.anisotropicFiltering = anisotropicFiltering;

        anisotropicFilteringLabel.text = "Anisotropic Filtering*";
        isHasEditSettingsAnisotropicFiltering = true;
    }

    public void AntiAliazing(int currentAntiAliazing)
    {
        switch (currentAntiAliazing) 
        {
            case 0:
                hdAdditionalCameraData.antialiasing = AntialiasingMode.None;

                antiAliazingDropdown.interactable = false;
                dlssDropdown.interactable = true;

                antiAliazingLabel.text = "Anti Aliazing*";
                isHasEditSettingsAntiAliazing = true;
                break;

            case 1:
                hdAdditionalCameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
                ChangeAntiAliazing();
                break;

            case 2:
                hdAdditionalCameraData.antialiasing = AntialiasingMode.TemporalAntialiasing;
                ChangeAntiAliazing();
                break;

            case 3:
                hdAdditionalCameraData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                ChangeAntiAliazing();
                break;
        }
    }

    public void ChangeAntiAliazing() 
    {
        antiAliazingDropdown.interactable = true;
        dlssDropdown.interactable = false;

        hdAdditionalCameraData.allowDeepLearningSuperSampling = false;

        antiAliazingLabel.text = "Anti Aliazing*";
        isHasEditSettingsAntiAliazing = true;
    }

    public void SaveGraphicsSettings() 
    {
        PlayerPrefs.SetInt(CommonSettingsData.SettingsKeys.QualitySettingsKey, qualityDropdown.value);
        PlayerPrefs.SetInt(CommonSettingsData.SettingsKeys.DisplaySettingsKey, displayDropdown.value);
        PlayerPrefs.SetInt(CommonSettingsData.SettingsKeys.ResolutionSettingsKey, resolutionDropdown.value);
        PlayerPrefs.SetInt(CommonSettingsData.SettingsKeys.DLSSSettingsKey, dlssDropdown.value);
        PlayerPrefs.SetInt(CommonSettingsData.SettingsKeys.AnisotropicFilteringKey, anisotropicFilteringDropdown.value);
        PlayerPrefs.SetInt(CommonSettingsData.SettingsKeys.AntiAlizingKey, antiAliazingDropdown.value);

        panelConfirmationWindowForSaves.SetActive(false);

        RemoveSpecialSignAfterEditSettings();
    }

    public void RemoveSpecialSignAfterEditSettings() // убрать знак "*"
    {
        qualityTextLabel.text = "Quality";
        displayTextLabel.text = "Display";
        resolutionTextLabel.text = "Resolution";
        dlssTextLabel.text = "DLSS";
        anisotropicFilteringLabel.text = "Anisotropic Filtering";
        antiAliazingLabel.text = "Anti Aliazing";
        
        isHasEditSettingQuality = false;
        isHasEditSettingDisplay = false;
        isHasEditSettingResolution = false;
        isHasEditSettingDLSS = false;
        isHasEditSettingsAnisotropicFiltering = false;
        isHasEditSettingsAntiAliazing = false;
    }

    public void LoadGraphicsSettings(int currentResolutionIndex) 
    {
        if (PlayerPrefs.HasKey(CommonSettingsData.SettingsKeys.QualitySettingsKey))
            qualityDropdown.value = PlayerPrefs.GetInt(CommonSettingsData.SettingsKeys.QualitySettingsKey);
        else
            qualityDropdown.value = 4;

        if (PlayerPrefs.HasKey(CommonSettingsData.SettingsKeys.DisplaySettingsKey))
            displayDropdown.value = PlayerPrefs.GetInt(CommonSettingsData.SettingsKeys.DisplaySettingsKey);
        else
            displayDropdown.value = 3;

        if (PlayerPrefs.HasKey(CommonSettingsData.SettingsKeys.ResolutionSettingsKey))
            resolutionDropdown.value = PlayerPrefs.GetInt(CommonSettingsData.SettingsKeys.ResolutionSettingsKey);
        else
            resolutionDropdown.value = currentResolutionIndex;

        if (PlayerPrefs.HasKey(CommonSettingsData.SettingsKeys.DLSSSettingsKey))
            dlssDropdown.value = PlayerPrefs.GetInt(CommonSettingsData.SettingsKeys.DLSSSettingsKey);
        else
            dlssDropdown.value = 0;

        if (PlayerPrefs.HasKey(CommonSettingsData.SettingsKeys.AnisotropicFilteringKey))
            anisotropicFilteringDropdown.value = PlayerPrefs.GetInt(CommonSettingsData.SettingsKeys.AnisotropicFilteringKey);
        else
            anisotropicFilteringDropdown.value = 0;

        if (PlayerPrefs.HasKey(CommonSettingsData.SettingsKeys.AntiAlizingKey))
            antiAliazingDropdown.value = PlayerPrefs.GetInt(CommonSettingsData.SettingsKeys.AntiAlizingKey);
        else
            antiAliazingDropdown.value = 0;

        RemoveSpecialSignAfterEditSettings();
    }

    public void SaveFPSCounterONInScenes() 
    {
        fPSDisplay.textFPS.gameObject.SetActive(true);

        showFpsToggle = 1;
        PlayerPrefs.SetInt(CommonSettingsData.SettingsKeys.ShowFPSToggleKey, showFpsToggle);

        showFpsCounter = 1;
        PlayerPrefs.SetInt(CommonSettingsData.SettingsKeys.ShowFPSCounterKey, showFpsCounter);

        PlayerPrefs.DeleteKey(CommonSettingsData.SettingsKeys.HideFPSToggleKey);
        PlayerPrefs.DeleteKey(CommonSettingsData.SettingsKeys.HideFPSCounterKey);
    }

    public void SaveFPSCounterOFFInScenes()
    {
        fPSDisplay.textFPS.gameObject.SetActive(false);

        showFpsToggle = 0;
        PlayerPrefs.SetInt(CommonSettingsData.SettingsKeys.HideFPSToggleKey, showFpsToggle);

        showFpsCounter = 0;
        PlayerPrefs.SetInt(CommonSettingsData.SettingsKeys.HideFPSCounterKey, showFpsCounter);

        PlayerPrefs.DeleteKey(CommonSettingsData.SettingsKeys.ShowFPSToggleKey);
        PlayerPrefs.DeleteKey(CommonSettingsData.SettingsKeys.ShowFPSCounterKey);
    }

    public void LoadFPSCounterONInScenes() 
    {
        showFpsToggle = PlayerPrefs.GetInt(CommonSettingsData.SettingsKeys.ShowFPSToggleKey);
        showFpsCounter = PlayerPrefs.GetInt(CommonSettingsData.SettingsKeys.ShowFPSCounterKey);

        fPSDisplay.textFPS.gameObject.SetActive(true);
        fpsToggle.isOn = true;
    }

    public void LoadFPSCounterOFFInScenes() 
    {
        showFpsToggle = PlayerPrefs.GetInt(CommonSettingsData.SettingsKeys.HideFPSToggleKey);
        showFpsCounter = PlayerPrefs.GetInt(CommonSettingsData.SettingsKeys.HideFPSCounterKey);

        fPSDisplay.textFPS.gameObject.SetActive(false);
        fpsToggle.isOn = false;
    }

    public void CheckKeysFPSForPlayerPrefs() 
    {
        if (PlayerPrefs.HasKey(CommonSettingsData.SettingsKeys.ShowFPSToggleKey) && PlayerPrefs.HasKey(CommonSettingsData.SettingsKeys.ShowFPSCounterKey))
            LoadFPSCounterONInScenes();
        

        else if (PlayerPrefs.HasKey(CommonSettingsData.SettingsKeys.HideFPSToggleKey) && PlayerPrefs.HasKey(CommonSettingsData.SettingsKeys.HideFPSCounterKey))
            LoadFPSCounterOFFInScenes();
        
    }

    public void ClickButtonNoForSavesSettings() => panelConfirmationWindowForSaves.SetActive(false);

}
