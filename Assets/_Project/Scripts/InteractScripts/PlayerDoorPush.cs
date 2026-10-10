using UnityEngine;

public class PlayerDoorPush : MonoBehaviour
{
    [SerializeField] float pushForce = 2f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null) return; // if on object not have a component Rigidbody, when do nothing

        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);

        body.AddForceAtPosition(pushDirection * pushForce, hit.point, ForceMode.Impulse);
    }
}
