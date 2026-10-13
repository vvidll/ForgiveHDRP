using _Project.Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SettingsAnimationCarAndPlayerPositionStartGame : MonoBehaviour
{
    [Header("Before change position and rotation player\nYOU NEED SAVE DATA In <ContextMenu> -> three points")]

    [Header("Through <ContextMenu> -> three points on this script\nON/OFF Start Animation and Player position before Start Game")]

    [Header("Animator")]
    [SerializeField] Animator playerAnimator;

    [Header("Car Animator")]
    [SerializeField] Animator carAnimator;

    [Header("Set Parent For Player")]
    [SerializeField] Transform player;
    [SerializeField] Transform parent;
    [SerializeField] Transform parentCar;

    [Header("Teleport Player In Car")]
    [SerializeField] Transform pointTeleportPlayerInCar;

    [Header("RigBuilder")]
    [SerializeField] RigBuilder rigBuilder;

    [Header("RigBuilder")]
    [SerializeField] DrivingPlayer drivingPlayer;

    Vector3 currentPositionPlayer;
    Quaternion currentRotationPlayer;

    int isSaveRotationAndPosition = 0;

    const string SaveRotationAndPositionKey = "Save_Rotation_And_Position";

    private void Awake()
    {
        if (isSaveRotationAndPosition == 0)
            Debug.LogError("SAVE THE DATA IN Script: --GAME_MANAGER-- In Hierarchy -> 'SettingsAnimationCarAndPlayerPositionStartGame' -> 'three points ON the script' -> 'SAVE Player position and Rotation' ");
    }

    [ContextMenu("LOAD Player position and Rotation")]
    void LoadPositionAndRotationPlayer() 
    {
        if (PlayerPrefs.HasKey(SaveRotationAndPositionKey)) 
        {
            isSaveRotationAndPosition = PlayerPrefs.GetInt(SaveRotationAndPositionKey);

            player.position = currentPositionPlayer;
            player.rotation = currentRotationPlayer;

            isSaveRotationAndPosition = 0;
        }
    }

    [ContextMenu("SAVE Player position and Rotation")]
    void SavePositionAndRotationPlayer() 
    {
        currentPositionPlayer = player.position;
        currentRotationPlayer = player.rotation;

        isSaveRotationAndPosition = 1;

        PlayerPrefs.SetInt(SaveRotationAndPositionKey, isSaveRotationAndPosition);
    }

    [ContextMenu("ON Start Animation and Player position")]
    void OnAnimation() 
    {
        // start the game -> on in play mode for animation work
        playerAnimator.SetTrigger("isDrivingStartGame");

        player.position = pointTeleportPlayerInCar.position;
        player.rotation = new Quaternion(0f, 180f, 0f, 0f);
        player.SetParent(parentCar);

        drivingPlayer.isInCar = true;

        for (int i = 1; i < rigBuilder.layers.Count && i < 5; i++)
        {
            rigBuilder.layers[i].active = true;
        }

        carAnimator.SetTrigger("isStartAnimation");
    }

    [ContextMenu("OFF Start Animation and Player position")]
    void OffAnimtion() 
    {
        drivingPlayer.isInCar = false;

        player.SetParent(parent);

        LoadPositionAndRotationPlayer();

        for (int i = 1; i < rigBuilder.layers.Count && i < 5; i++)
        {
            rigBuilder.layers[i].active = false;
        }
    }
}
