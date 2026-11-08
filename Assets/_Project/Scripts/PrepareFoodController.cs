using System;
using System.Collections;
using _Project.Scripts.MissionsScripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PrepareFoodController : MonoBehaviour
{
    [SerializeField] ShowMissionsManager missionManager;

    int timerPrepareFood = 2000; // two seconds

    private void Update()
    {
        if (Input.GetMouseButton(3) && missionManager.isHasPrepareFood == true) 
        {
            // сделать анимацию готовки еды

            ShowMissionAfterPrepareFoodCoroutine().Forget();
        }
    }

    async UniTaskVoid ShowMissionAfterPrepareFoodCoroutine() 
    {
        try
        {
            await UniTask.Delay(timerPrepareFood, cancellationToken: this.GetCancellationTokenOnDestroy());
            missionManager.ShowMissionPutSleepingBag();
        }
        catch (Exception ex) { Debug.LogError(ex); }

    }
}
