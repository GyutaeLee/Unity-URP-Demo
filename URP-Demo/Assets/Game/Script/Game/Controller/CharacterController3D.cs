using UnityEngine;

namespace Demo.Game.Controller
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class CharacterController3D : MonoBehaviour
    {
        [SerializeField]
        private CharacterController characterController;

        [SerializeField]
        private float speed = 10.0f;
        [SerializeField]
        private float jumpHeight = 10.0f;

        [SerializeField]
        private float gravity = -9.81f;

        [SerializeField]
        private float groundDistance = 0.4f;
        [SerializeField]
        private Transform groundChecker;
        [SerializeField]
        private LayerMask groundLayerMask;

        private bool isGrounded;

        private Vector3 move;
        private Vector3 force;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void FixedUpdate()
        {
            this.characterController.Move(this.speed * this.move);
            this.isGrounded = Physics.CheckSphere(this.groundChecker.position, this.groundDistance, this.groundLayerMask);
            if (this.isGrounded == true && this.force.y < 0)
            {
                this.force.y = -2.0f;
            }

            this.force.y += this.gravity;
            this.characterController.Move(this.force);
        }

        public void Move(float x, float z)
        {
            this.move = transform.right * x + transform.forward * z;
        }

        public void Jump()
        {
            if (this.isGrounded == true)
            {
                this.force.y = Mathf.Sqrt(this.jumpHeight * -2.0f * this.gravity);
            }
        }

        public void Attack()
        {

        }
    }
}
