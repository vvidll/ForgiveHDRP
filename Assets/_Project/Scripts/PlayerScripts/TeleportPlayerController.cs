using System.Collections;
using UnityEngine;

public class TeleportPlayerController : MonoBehaviour
{
    [SerializeField] Transform teleportPointAfterFailedTry;

    [SerializeField] Transform player;

    public bool isCanTeleport = false;

    int timer = 1;
    int currentTime = 3;

    private void Update()
    {
        StartCoroutine(WarningTimerCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
            isCanTeleport = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            isCanTeleport = false;
        
    }

    IEnumerator WarningTimerCoroutine() 
    {
        if (isCanTeleport == true) 
        {
            while (currentTime <= 3)
            {
                currentTime--;

                yield return new WaitForSeconds(timer);

                if (currentTime <= 0)
                {
                    currentTime = 0;
                    isCanTeleport = false;

                    player.position = teleportPointAfterFailedTry.position;
                }

                Debug.Log(currentTime);
                Debug.Log(isCanTeleport);
            }
        }
    }

}
