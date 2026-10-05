using _Project.Scripts.PlacedItemsScripts;
using UnityEngine;

public class AudioNightManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSourceHorrorMusic;
    [SerializeField] SleepingBagController sleepingBagController;

    private void Update()
    {
        // если наступила ночь
        if (sleepingBagController.isChangeTimeDay == true) 
        {
            // временно отключил
            //if(audioSourceHorrorMusic.isPlaying) return;
            //audioSourceHorrorMusic.Play();
        }
    }
}
