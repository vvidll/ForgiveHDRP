using System.Collections;
using _Project.Scripts;
using TMPro;
using UnityEngine;

public class TeleportPlayerController : MonoBehaviour
{
    [SerializeField] TransitionsController transitionsController;

    [SerializeField] GameObject warningMessageUI;
    [SerializeField] TMP_Text timerText;

    [SerializeField] Transform teleportPointAfterFailedTry;

    [SerializeField] Transform player;

    public bool isCanTeleport = false;

    int maxTime = 5;

    int currentTime = 0;

    int interval = 1;

    bool startCoroutine = false;

    private void Start()
    {
        currentTime = maxTime;
    }

    private void Update()
    {
        if (isCanTeleport == true && startCoroutine == false) 
        {
            StartCoroutine(WarningTimerCoroutine());
        }

        if(isCanTeleport == false && startCoroutine == true && currentTime != 0)
        {
            currentTime = maxTime;
            timerText.text = currentTime.ToString();

            player.position = this.transform.position;

            Debug.Log("Not teleporting!");

            startCoroutine = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CloseZone")
        {
            warningMessageUI.SetActive(true);

            isCanTeleport = true;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CloseZone") 
        {
            warningMessageUI.SetActive(false);

            isCanTeleport = false;
        }

        
    }

    IEnumerator WarningTimerCoroutine() 
    {
        startCoroutine = true;

        while (currentTime > 0 && isCanTeleport == true)
        {
            yield return new WaitForSeconds(interval);

            currentTime--;

            if (currentTime <= 0) 
            {
                currentTime = 0;
                isCanTeleport = false;
            }

            timerText.text = currentTime.ToString();
        }

        if (isCanTeleport == false && currentTime == 0)
        {
            transitionsController.StartCoroutine(transitionsController.EndTimerCoroutine());

            player.position = teleportPointAfterFailedTry.position;

            Debug.Log("We teleporting!");

            startCoroutine = false;

            currentTime = maxTime;
            timerText.text = currentTime.ToString();
        }
    }

}
