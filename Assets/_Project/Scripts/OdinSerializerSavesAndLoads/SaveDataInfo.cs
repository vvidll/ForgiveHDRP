using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections;

public class SaveDataInfo : MonoBehaviour
{
    [SerializeField] TMP_Text saveDataInfoText;

    float timeCoroutineEnd = .1f;
    float timeChangeColorText = 3f;

    public void InfoActive() 
    { 
        saveDataInfoText.DOColor(new Color(255, 255, 255, 255), timeChangeColorText);
        StartCoroutine(DisableInfoCoroutine());
    }
    
    void InfoNonActive() => saveDataInfoText.DOColor(new Color(255, 255, 255, 0), timeChangeColorText);

    IEnumerator DisableInfoCoroutine() 
    {
        yield return new WaitForSeconds(timeCoroutineEnd);
        InfoNonActive();
    }

}
