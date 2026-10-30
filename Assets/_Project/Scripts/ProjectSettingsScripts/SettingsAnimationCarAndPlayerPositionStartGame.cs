using _Project.Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SettingsAnimationCarAndPlayerPositionStartGame : MonoBehaviour
{
    [Header("Before change position and rotation player\n" +
        "YOU NEED SAVE DATA In <ContextMenu> -> three points")]

    [Header("Through <ContextMenu> -> three points on this script\n " +
        "< ON > Start Animation and Player position enable in Play Mode\n " +
        "< OFF > Start Animation and Player position before Start Game")]

    [Header("Animator")]
    [SerializeField] Animator playerAnimator;

    [Header("Car Animator")]
    [SerializeField] Animator carAnimator;

    [Header("Set Parent For Player")]
    [SerializeField] Transform player;
    [SerializeField] Transform parent;
    [SerializeField] Transform parentCar;

    [Header("Start Point")]
    [SerializeField] Transform pointStartGame;

    [Header("Teleport Player In Car")]
    [SerializeField] Transform pointTeleportPlayerInCar;

    [Header("RigBuilder")]
    [SerializeField] RigBuilder rigBuilder;

    [Header("RigBuilder")]
    [SerializeField] DrivingPlayer drivingPlayer;

    Vector3 currentPositionPlayer;
    Quaternion currentRotationPlayer;

    Vector3 currentPositionCar;
    Quaternion currentRotationCar;

    int isSaveRotationAndPosition = 0;

    const string SaveRotationAndPositionKey = "Save_Rotation_And_Position";

    public bool isAnimationEnabled = false;

    private void Awake()
    {
        if (isAnimationEnabled == true) 
        {
            carAnimator.SetTrigger("isStartAnimation");
            playerAnimator.SetTrigger("isDrivingStartGame");
        }

        if (isSaveRotationAndPosition == 0)
            Debug.LogError("SAVE THE DATA IN Script: --GAME_MANAGER-- In Hierarchy -> 'SettingsAnimationCarAndPlayerPositionStartGame' -> 'three points ON the script' -> 'SAVE Player position and Rotation' ");
    }

    [ContextMenu("LOAD Player/Car position and Rotation")]
    void LoadPositionAndRotationPlayer() 
    {
        if (PlayerPrefs.HasKey(SaveRotationAndPositionKey)) 
        {
            isSaveRotationAndPosition = PlayerPrefs.GetInt(SaveRotationAndPositionKey);

            player.position = currentPositionPlayer;
            player.rotation = currentRotationPlayer;

            parentCar.position = currentPositionCar;
            parentCar.rotation = currentRotationCar;

            isSaveRotationAndPosition = 0;
        }
    }

    [ContextMenu("SAVE Player/Car position and Rotation")]
    void SavePositionAndRotationPlayer() 
    {
        currentPositionPlayer = player.position;
        currentRotationPlayer = player.rotation;

        currentPositionCar = parentCar.position;
        currentRotationCar = parentCar.rotation;

        isSaveRotationAndPosition = 1;

        PlayerPrefs.SetInt(SaveRotationAndPositionKey, isSaveRotationAndPosition);
    }

    [ContextMenu("ON Start Animation Car and Player position")]
    void OnAnimation() 
    {
        // start the game -> on in play mode for animation work

        isAnimationEnabled = true;

        parentCar.gameObject.SetActive(true);

        parentCar.position = pointStartGame.position;

        parentCar.position = pointStartGame.position;
        parentCar.rotation = new Quaternion(0f, 0f, 0f, 1f);

        player.SetParent(parentCar);

        player.position = pointTeleportPlayerInCar.position;
        player.rotation = new Quaternion(0f, .7f, 0f, .7f);


        drivingPlayer.isInCar = true;

        for (int i = 1; i < rigBuilder.layers.Count && i < 5; i++)
        {
            rigBuilder.layers[i].active = true;
        }

        carAnimator.SetTrigger("isStartAnimation");
    }

    [ContextMenu("OFF Start Animation Car and Player position")]
    void OffAnimtion() 
    {
        isAnimationEnabled = false;

        drivingPlayer.isInCar = false;

        player.SetParent(parent);

        LoadPositionAndRotationPlayer();

        for (int i = 1; i < rigBuilder.layers.Count && i < 5; i++)
        {
            rigBuilder.layers[i].active = false;
        }
    }
}
