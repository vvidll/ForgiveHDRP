using UnityEngine;

namespace _Project.Scripts.AudioScripts
{
    // скрипт отвечающий за общую работу звуков(не доделан)
    public class AudioMoveManager : MonoBehaviour
    {
        [Header("Audio Grass")]
        [SerializeField] AudioSource audioRandomContainerGrassWalk;
        [SerializeField] AudioSource audioRandomContainerGrassRun;
        [SerializeField] AudioSource audioRandomContainerGrassJump;
        [SerializeField] AudioSource audioRandomContainerGrassStartJump;

        [Header("Audio Gravel")]
        [SerializeField] AudioSource audioRandomContainerGravelWalk;
        [SerializeField] AudioSource audioRandomContainerGravelRun;
        [SerializeField] AudioSource audioRandomContainerGravelJump;
        [SerializeField] AudioSource audioRandomContainerGravelStartJump;

        public void PlayAudioForGrassWalk()
        {
            if (audioRandomContainerGrassRun.isPlaying) return;
            audioRandomContainerGrassWalk.PlayDelayed(0.01f);
        }

        public void PlayAudioForGravelWalk()
        {
            if (audioRandomContainerGravelWalk.isPlaying) return;
            audioRandomContainerGravelWalk.PlayDelayed(0.01f);
        }

        public void StopAudioForGrassWalk() => audioRandomContainerGrassWalk.Stop();

        public void StopAudioForGravelWalk() => audioRandomContainerGravelWalk.Stop();


        public void PlayAudioForGrassRun()
        {
            if (audioRandomContainerGrassRun.isPlaying) return;
            audioRandomContainerGrassRun.PlayDelayed(0.01f);
        }

        public void PlayAudioForGravelRun()
        {
            if (audioRandomContainerGravelRun.isPlaying) return;
            audioRandomContainerGravelRun.PlayDelayed(0.05f);
        }

        public void PlayAudioForGrassJump()
        {
            if (audioRandomContainerGrassJump.isPlaying) return;
            audioRandomContainerGrassJump.Play();
        }

        public void PlayAudioForGravelJump()
        {
            if (audioRandomContainerGravelJump.isPlaying) return;
            audioRandomContainerGravelJump.Play();
        }

        public void PlayAudioForGrassStartJump()
        {
            if (audioRandomContainerGrassStartJump.isPlaying) return;
            audioRandomContainerGrassStartJump.Play();
        }

        public void PlayAudioForGravelStartJump()
        {
            if (audioRandomContainerGravelStartJump.isPlaying) return;
            audioRandomContainerGravelStartJump.Play();
        }
    }
}
