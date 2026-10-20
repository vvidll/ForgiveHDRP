using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.MainMenuScripts
{
    public class MainMenu : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] TMP_Text settingsTmpText;
        [SerializeField] SpriteRenderer settingsSpriteRenderer;

        [SerializeField] TMP_Text loadGamesTmpText;
        [SerializeField] SpriteRenderer loadGamesSpriteRenderer;

        public GameObject confirmationWindowForNewGame;
        public GameObject confirmationWindowForExitGame;

        //public GameObject panelMenuSavesGames;

        public TMP_Text nameGame;
        public GameObject panelMenuButtons;

        //public GameObject loadGameWindow;
        public GameObject settingsWindow;
        public GameObject authorsWindow;

        [SerializeField] GameObject panelConfirmationWindowForSaves;

        [Header("Camera")]
        [SerializeField] Transform mainCamera;

        [Header("References")]
        [SerializeField] SettingsGraphics settingsGraphics;
        [SerializeField] SettingsVolumeAmbientOcclusion postProcess;
        [SerializeField] SettingsVolumeFogQuality postProcessFogQuality;
        [SerializeField] SettingsShadowMapQuality settingsShadowMapQuality;
        [SerializeField] SettingsContactShadows settingsContactShadows;
        [SerializeField] SettingsVolumeCascadedShadowMaps postProcessShadowsQuality;
        [SerializeField] SettingVolumeMicroShadows postProcessMicroShadows;
        [SerializeField] SettingsVolumeLensFlare postProcessLensFlare;
        [SerializeField] SettingsRain settingsRain;

        public bool isPressEscInMainMenuForExitGame = true;
        public bool isPressEscForReturnInMainMenu = false;

        //bool isCanMove = false;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && isPressEscInMainMenuForExitGame == true && isPressEscForReturnInMainMenu == false &&
                settingsWindow.activeSelf == false && 
                authorsWindow.activeSelf == false)
                ExitGame();

            if (Input.GetKey(KeyCode.H))
                Settings();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                /*if (loadGameWindow.activeSelf == true)
                {
                    loadGameWindow.SetActive(false);
                    ShowMainMenu();
                    panelMenuSavesGames.SetActive(false);
                    SetBaseColor();
                }*/

                if (settingsWindow.activeSelf == true)
                {
                    
                    if (settingsGraphics.isHasEditSettingDisplay == true || settingsGraphics.isHasEditSettingResolution == true ||
                    settingsGraphics.isHasEditSettingQuality == true || settingsGraphics.isHasEditSettingDLSS == true ||
                    postProcess.isHasEditAmbientOcclusionQuality == true || settingsGraphics.isHasEditSettingsAnisotropicFiltering == true || postProcessFogQuality.isHasEditSettingsFogQuality == true ||
                    postProcessShadowsQuality.isHasEditCascadedShadowMapsSettings == true || postProcessMicroShadows.isHasEditSettingsMicroShadows == true || 
                    postProcessLensFlare.isHasEditSettingsLensFlare == true || settingsGraphics.isHasEditSettingsAntiAliazing == true || 
                    settingsShadowMapQuality.isHasEditSetgginsShadowMapQuality == true || settingsContactShadows.isHasEditSettingsContactShadows == true ||
                    settingsRain.isHasEditSettingsRain == true)
                    {
                        panelConfirmationWindowForSaves.SetActive(true);
                    }

                    else if (settingsGraphics.isHasEditSettingDisplay == false || settingsGraphics.isHasEditSettingResolution == false ||
                    settingsGraphics.isHasEditSettingQuality == false || settingsGraphics.isHasEditSettingDLSS == false ||
                    postProcess.isHasEditAmbientOcclusionQuality == false || settingsGraphics.isHasEditSettingsAnisotropicFiltering == false || postProcessFogQuality.isHasEditSettingsFogQuality == false ||
                    postProcessShadowsQuality.isHasEditCascadedShadowMapsSettings == false || postProcessMicroShadows.isHasEditSettingsMicroShadows == false || 
                    postProcessLensFlare.isHasEditSettingsLensFlare == false || settingsGraphics.isHasEditSettingsAntiAliazing == false || 
                    settingsShadowMapQuality.isHasEditSetgginsShadowMapQuality == false || settingsContactShadows.isHasEditSettingsContactShadows == false ||
                    settingsRain.isHasEditSettingsRain == false) 
                    {
                        settingsWindow.SetActive(false);
                        SetBaseColor();
                        ShowMainMenu();
                    }
                }

                else if (authorsWindow.activeSelf == true) 
                {
                    authorsWindow.SetActive(false);
                    ShowMainMenu();
                }
            }
        }

        public void ExitGame() 
        {
            confirmationWindowForExitGame.gameObject.SetActive(true);
            HideMainMenu();
        }

        public void LoadGameControlPoint()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void Settings() 
        {
            settingsWindow.SetActive(true);
            HideMainMenu();
        }

        public void SetBaseColor()
        {
            settingsTmpText.DOColor(new Color(255, 255, 255, 255), 0.2f);
            settingsSpriteRenderer.DOColor(new Color(255, 255, 255, 255), 0.2f);
        }

        public void HideMainMenu() 
        {
            nameGame.gameObject.SetActive(false);
            panelMenuButtons.SetActive(false);

            isPressEscInMainMenuForExitGame = false;
            isPressEscForReturnInMainMenu = true;
        }

        public void ShowMainMenu() 
        {
            nameGame.gameObject.SetActive(true);
            panelMenuButtons.SetActive(true);

            isPressEscInMainMenuForExitGame = true;
            isPressEscForReturnInMainMenu = false;
        }
    }
}
