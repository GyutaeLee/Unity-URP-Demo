using UnityEngine;

namespace Demo.Game.Controller.Player
{
    public sealed class PlayerController : Game.Controller.BaseController
    {
        private AxisAction walk;
        private AxisAction sprint;
        private Action crouch;
        private Action jump;

        private void Awake()
        {
            this.context = new Context
            {
                animator = this.transform.GetComponent<Animator>(),
                rigidbody = this._rigidbody,
                transform = this.transform,
                originalScale = this.transform.localScale
            };

            this.walk = new Walk(this.context, "isRun");
            this.sprint = new Sprint(this.context, "isRun");
            this.crouch = new Crouch(this.context, "isCrouch");
            this.jump = new Jump(this.context, "JumpingTrigger");
        }

        private void Update()
        {
            CheckIsGrounded();
        }

        private void CheckIsGrounded()
        {
            Vector3 origin = new Vector3(this.transform.position.x,
                                         this.transform.position.y + 0.01875f,
                                         this.transform.position.z);
            Vector3 direction = this.transform.TransformDirection(Vector3.down);
            float raycastDistance = 0.05f;

            Debug.DrawRay(origin, direction * raycastDistance, Color.blue);
            if (Physics.Raycast(origin, direction, out RaycastHit hit, raycastDistance) == true)
            {
                this.context.isGrounded = true;
            }
            else
            {
                this.context.isGrounded = false;
            }
        }

        public void DoWalk(float x, float z)
            => this.walk.Run(x, z);

        public void DoSprint(float x, float z)
            => this.sprint.Run(x, z);

        public void DoCrouch()
            => this.crouch.Run();

        public void DoJump()
        {
            if (this.context.isGrounded == true)
            {
                this.jump.Run();
            }

            if (this.context.isCrouched == true)
            {
                this.crouch.Run();
            }
        }
    }
}
