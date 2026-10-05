using System.Collections;
using UnityEngine;

public class AudioCarController : MonoBehaviour
{
    [SerializeField] AudioSource audioCarSignaling;

    public void ActivateAudio() => StartCoroutine(PlayAudioCoroutine());

    IEnumerator PlayAudioCoroutine() 
    {
        yield return new WaitForSeconds(1f);
        audioCarSignaling.Play();
    }
}
