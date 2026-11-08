using UnityEngine;
using DG.Tweening;
using TMPro;
using Cysharp.Threading.Tasks;
using System;

public class SaveDataInfo : MonoBehaviour
{
    [SerializeField] TMP_Text saveDataInfoText;

    int timeCoroutineEnd = 100;
    float timeChangeColorText = 3f;

    public void InfoActive() 
    { 
        saveDataInfoText.DOColor(new Color(255, 255, 255, 255), timeChangeColorText);
        DisableInfoCoroutine().Forget();
    }
    
    void InfoNonActive() => saveDataInfoText.DOColor(new Color(255, 255, 255, 0), timeChangeColorText);

    async UniTaskVoid DisableInfoCoroutine() 
    {
        try 
        { 
            await UniTask.Delay(timeCoroutineEnd, cancellationToken: this.GetCancellationTokenOnDestroy());
            InfoNonActive();
        }
        catch (Exception ex) { Debug.LogError(ex); }
    }

}
