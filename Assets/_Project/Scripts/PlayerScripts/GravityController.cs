using UnityEngine;

namespace _Project.Scripts.PlayerScripts
{
    [RequireComponent(typeof(CharacterController))]
    public class GravityController : MonoBehaviour
    {
        [SerializeField] CharacterController characterController;

        [SerializeField] Transform groundCheck;
        [SerializeField] LayerMask groundMask;
        [SerializeField] float groundDistance = 0.4f;

        protected float Gravity = -9.81f;

        public bool IsGrounded;
        protected Vector3 Velocity;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        public void CheckGravity()
        {
            IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (IsGrounded == true && Velocity.y < 0)
                Velocity.y = -2f;

            Velocity.y += Gravity * Time.deltaTime;

            characterController.Move(Velocity * Time.deltaTime);
        }



    }
}
