using _Project.Scripts.AudioScripts;
using UnityEngine;

public class TerrainLayersController : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] TerrainLayer[] terrainLayers;

    [SerializeField] Transform groundCheck;
    //[SerializeField] BoxCollider2D groundCheckCollider;

    [SerializeField] AudioMoveManager audioManager;

    public void RayCheckLayer() 
    {
        RaycastHit hit;

        if (Physics.Raycast(groundCheck.position, Vector3.down, out hit)) 
        {


            /*if (hit.collider != null && terrainLayers[0].name == "Grass_A")
            {
                Debug.Log("Луч коснулся земли!");
            }

            if (hit.collider != null && terrainLayers[1].name == "Black_Sand_A")
            {
                Debug.Log("Луч коснулся гравия!");
            }*/

            for (int i = 0; i < terrainLayers.Length; i++)
            {
                //if (hit.collider.la) { }
            }
        }
    }

    public void CheckLayerForWalk() 
    {
        for(int i = 0; i < terrainLayers.Length; i++) 
        {
            switch (terrainLayers[i].name)
            {
                /*case "Grass_A":
                    audioManager.PlayAudioForGrassWalk();
                    break;
                */
                /*case "Black_Sand_A":
                    audioManager.PlayAudioForGravelWalk(); 
                    break;*/
            }
        }
    }

    public void CheckLayerForRun() 
    {
        for (int i = 0; i < terrainLayers.Length; i++)
        {
            switch (terrainLayers[i].name)
            {
                /*case "Grass_A":
                    audioManager.PlayAudioForGrassRun();
                    break;
                */
                /*case "Black_Sand_A":
                    audioManager.PlayAudioForGravelRun();
                    break;*/
            }
        }
    }
}
