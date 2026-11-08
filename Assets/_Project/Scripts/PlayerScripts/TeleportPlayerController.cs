using System.Collections;
using _Project.Scripts;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.PlayerScripts
{
    public class TeleportPlayerController : MonoBehaviour
    {
        [SerializeField] TransitionsController transitionsController;

        [SerializeField] GameObject warningMessageUI;
        [SerializeField] TMP_Text timerText;

        [SerializeField] Transform[] teleportPointsAfterFailedTry;

        [SerializeField] CharacterController player;

        public bool isCanTeleport = false;

        int maxTime = 5;

        int currentTime = 0;

        int interval = 1;

        bool startTask = false;

        private void Start()
        {
            currentTime = maxTime;
        }

        private void Update()
        {
            if (isCanTeleport == true && startTask == false)
            {
                StartCoroutine(WarningTimerCoroutine());
            }

            if (isCanTeleport == false && startTask == true && currentTime != 0)
            {
                currentTime = maxTime;
                timerText.text = currentTime.ToString();

                player.transform.position = this.transform.position;

                Debug.Log("Not teleporting!");

                startTask = false;
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("CloseZone"))
            {
                warningMessageUI.SetActive(true);

                isCanTeleport = true;

            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("CloseZone"))
            {
                warningMessageUI.SetActive(false);

                isCanTeleport = false;
            }


        }

        // !!! dont change to UniTask,
        // else when exit from play mode in pause (Esc), happens memory overflow !!!
        IEnumerator WarningTimerCoroutine()
        {
            startTask = true;

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
                transitionsController.EndTimerCloseZoneCoroutine().Forget();

                player.enabled = false;
                player.transform.position = FindNearDistanceToPointTeleport().position;
                player.enabled = true;

                Debug.Log("We teleporting!");

                startTask = false;

                currentTime = maxTime;
                timerText.text = currentTime.ToString();

                warningMessageUI.SetActive(false);
            }
        }

        Transform FindNearDistanceToPointTeleport()
        {
            float currentDistance = 0f;

            float minDistance = float.MaxValue;

            Transform nearPoint = null;

            foreach (Transform point in teleportPointsAfterFailedTry)
            {
                currentDistance = Vector3.Distance(point.position, player.transform.position);

                Debug.Log("point distance to player:" + currentDistance);

                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    nearPoint = point;
                }
            }


            Debug.Log("min distance to player:" + minDistance);

            Debug.Log(nearPoint);

            return nearPoint;









        }

    }
}