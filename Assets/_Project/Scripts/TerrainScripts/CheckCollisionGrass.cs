using UnityEngine;

public class CheckCollisionGrass : MonoBehaviour
{// временно отключил, т.к не ясно как включить коллайдеры травы на террейне(там можно вкл\откл только коллайдеры деревьев)

    /*float minScaleY = .1f, maxScaleY = 1f;
    float currentScaleY = 0;

    bool isEnterCollisionGrass = false;

    private void Awake()
    {
        currentScaleY = maxScaleY;
    }

    private void Update()
    {
        if (isEnterCollisionGrass == true) 
        {
            if (currentScaleY == maxScaleY)
            {
                currentScaleY--;
                this.transform.localScale = new Vector3(this.transform.localScale.x, currentScaleY, this.transform.localScale.z);

                if (currentScaleY <= minScaleY)
                    currentScaleY = minScaleY;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Car")
        {
            isEnterCollisionGrass = true;
            Debug.Log("Have Enter Grass");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Car") 
        {
            isEnterCollisionGrass = false;
            Debug.Log("Haven`t Enter Grass");
        }
    }*/
}
