using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Game.Controller
{
    [RequireComponent(typeof(Rigidbody))]
    public class BaseController : MonoBehaviour
    {
        [Serializable]
        private class Context
        {
            internal Vector3 originalScale;

            internal Rigidbody rigidbody;
            internal Transform transform;

            internal bool isGrounded;
            internal bool isCrouched;
            internal bool isSprinting;

            [SerializeField]
            internal float maxVelocityLimit = 10.0f;
            [SerializeField]
            internal float walkSpeed = 5.0f;
        }

        private abstract class Action
        {
            protected Context context;

            protected Action(Context context) { this.context = context; }

            public abstract void Run();
        }

        private abstract class AxisAction
        {
            protected Context context;

            protected AxisAction(Context context) { this.context = context; }

            public abstract void Run(float x, float y);
        }

        private class Crouch : Action
        {
            [SerializeField]
            private float crouchHeight = 0.75f;
            [SerializeField]
            private float speedReduction = 0.5f;

            public Crouch(Context context) : base(context)
            {
                // Check : Action 생성자에서 들어가고 있는지 확인하기
                //this.context = context;
            }

            public override void Run()
            {
                Vector3 changeLocalScale = this.context.originalScale;
                float speedReductionWeight = 1.0f;

                if (this.context.isCrouched == true)
                {
                    if (this.speedReduction > 0)
                    {
                        speedReductionWeight = (1 / this.speedReduction);
                    }
                }
                else
                {
                    speedReductionWeight = this.speedReduction;
                    changeLocalScale.y = crouchHeight;
                }

                this.context.transform.localScale = new Vector3(changeLocalScale.x, changeLocalScale.y, changeLocalScale.z);
                this.context.walkSpeed *= speedReductionWeight;
                this.context.isCrouched = !this.context.isCrouched;
            }
        }

        private class Jump : Action
        {
            [SerializeField]
            private float jumpPower = 5.0f;

            public Jump(Context context) : base(context)
            {
                // Check : Action 생성자에서 들어가고 있는지 확인하기
                //this.context = context;
            }

            public override void Run()
            {
                this.context.rigidbody.AddForce(0.0f, this.jumpPower, 0.0f, ForceMode.Impulse);
                this.context.isGrounded = false;
            }
        }

        private class Walk : AxisAction
        {
            public bool IsMovable { get; private set; }

            public Walk(Context context) : base(context)
            {
                // Check : Action 생성자에서 들어가고 있는지 확인하기
                //this.context = context;

                this.IsMovable = true;
            }

            public override void Run(float x, float y)
            {
                this.context.isSprinting = false;

                float maxVelocityLimit = this.context.maxVelocityLimit;
                float walkSpeed = this.context.walkSpeed;

                Vector3 walkDirection = this.context.transform.TransformDirection(new Vector3(x, 0, y));
                Vector3 rigidbodyVelocity = this.context.rigidbody.velocity;
                Vector3 walkForce = (walkDirection * walkSpeed - rigidbodyVelocity);
                walkForce.x = Mathf.Clamp(walkForce.x, -maxVelocityLimit, maxVelocityLimit);
                walkForce.y = 0.0f;
                walkForce.z = Mathf.Clamp(walkForce.z, -maxVelocityLimit, maxVelocityLimit);

                this.context.rigidbody.AddForce(walkForce, ForceMode.VelocityChange);
            }
        }

        private class Sprint : AxisAction
        {
            public float SprintSpeed { get; private set; }

            public Sprint(Context context) : base(context)
            {
                // Check : Action 생성자에서 들어가고 있는지 확인하기
                //this.context = context;

                this.SprintSpeed = 7.0f;
            }

            public override void Run(float x, float y)
            {
                float maxVelocityLimit = this.context.maxVelocityLimit;
                Vector3 sprintDirection = this.context.transform.TransformDirection(new Vector3(x, 0, y));

                Vector3 rigidbodyVelocity = this.context.rigidbody.velocity;
                Vector3 sprintForce = (sprintDirection - rigidbodyVelocity);
                sprintForce.x = Mathf.Clamp(sprintForce.x, -maxVelocityLimit, maxVelocityLimit);
                sprintForce.y = 0.0f;
                sprintForce.z = Mathf.Clamp(sprintForce.z, -maxVelocityLimit, maxVelocityLimit);

                this.context.rigidbody.AddForce(sprintForce, ForceMode.VelocityChange);
            }
        }

        [SerializeField]
        private Rigidbody _rigidbody;

        [SerializeField]
        private Context context;

        private AxisAction walk;
        private AxisAction sprint;
        private Action crouch;
        private Action jump;

        private void Awake()
        {
            this.context = new Context
            {
                rigidbody = this._rigidbody,
                transform = this.transform,
                originalScale = this.transform.localScale
            };

            this.walk = new Walk(this.context);
            this.sprint = new Sprint(this.context);
            this.crouch = new Crouch(this.context);
            this.jump = new Jump(this.context);
        }

        private void Update()
        {
            CheckGround();
        }

        private void CheckGround()
        {
            Vector3 origin = new Vector3(this.transform.position.x,
                                         this.transform.position.y - (this.transform.localScale.y * 0.5f),
                                         this.transform.position.z);
            Vector3 direction = this.transform.TransformDirection(Vector3.down);
            float raycastDistance = 0.75f;

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

        public void DoCroucch()
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