using System;
using _Project.Scripts.AudioScripts;
using _Project.Scripts.PlayerScripts;
using UnityEngine;

public class CheckLegsGround : MonoBehaviour
{
    bool isGrounded =  false;
    
    [SerializeField] LayerMask groundLayer;
    
    [SerializeField] AudioMoveManager audioManager;
    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] BoxCollider boxCollider;
    
    private void Update()
    {
        isGrounded = Physics.CheckBox(transform.position, boxCollider.bounds.extents, Quaternion.identity, groundLayer);

        if (isGrounded == true)
        {
            if(playerMovement.isHasWalking == true)
                EnableAudioForWalking();

            if(playerMovement.isHasRunning == true)
                EnableAudioForRunning();
        }

        else
            DisableAudioForWalking();
        
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            if(playerMovement.isHasWalking == true)
                EnableAudioForWalking();

            if(playerMovement.isHasRunning == true)
                EnableAudioForRunning();
        }
        
        DisableAudioForWalking();
    }*/

    public void EnableAudioForWalking() => audioManager.PlayAudioForGrassWalk();
        
    public void EnableAudioForRunning() => audioManager.PlayAudioForGrassRun();
    
    public void DisableAudioForWalking() => audioManager.StopAudioForGrassWalk();
}
