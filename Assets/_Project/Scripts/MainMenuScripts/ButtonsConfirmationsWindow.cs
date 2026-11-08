using DG.Tweening;
using _Project.Scripts.MainMenuScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsConfirmationsWindow : MonoBehaviour
{
    [SerializeField] TMP_Text newGameTmpText;
    [SerializeField] SpriteRenderer newGameSpriteRenderer;

    [SerializeField] TMP_Text exitGameTmpText;
    [SerializeField] SpriteRenderer exitGameSpriteRenderer;

    [SerializeField] TMP_Text currentTmpText;

    [SerializeField] MainMenu mainMenu;

    private void OnMouseDown()
    {
        switch (currentTmpText.name)
        {
            case "Text (TMP) YES_NEW_GAME":
                SceneManager.LoadScene("GameScene"); 
                mainMenu.isPressEscInMainMenuForExitGame = false;
                mainMenu.isPressEscForReturnInMainMenu = false;

                break;

            case "Text (TMP) NO_NEW_GAME":
                mainMenu.ShowMainMenu();
                mainMenu.confirmationWindowForNewGame.gameObject.SetActive(false);

                SetBaseColorForNewGame();
                break;

            case "Text (TMP) YES_EXIT_GAME":
                Application.Quit();
                Debug.Log("Произошел выход из игры");
                break;

            case "Text (TMP) NO_EXIT_GAME":
                mainMenu.ShowMainMenu();
                mainMenu.confirmationWindowForExitGame.gameObject.SetActive(false);

                SetBaseColorForExitGame();
                break;
        }
    }

    private void OnMouseEnter()
    {
        transform.DOScale(new Vector3(0.014f, 0.014f, 0.014f), 0.2f);
    }

    private void OnMouseExit()
    {
        transform.DOScale(new Vector3(0.015f, 0.015f, 0.015f), 0.2f);
    }

    public void SetBaseColorForNewGame() 
    {
        newGameTmpText.DOColor(new Color(255, 255, 255, 255), 0.2f);
        newGameSpriteRenderer.DOColor(new Color(255, 255, 255, 255), 0.2f);
    }

    public void SetBaseColorForExitGame() 
    {
        exitGameTmpText.DOColor(new Color(255, 255, 255, 255), 0.2f);
        exitGameSpriteRenderer.DOColor(new Color(255, 255, 255, 255), 0.2f);
    }
}
