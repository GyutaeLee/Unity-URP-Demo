using System;
using UnityEngine;

namespace Demo.Game.Controller
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class BaseController : MonoBehaviour
    {
        [Serializable]
        protected class Context
        {
            internal Vector3 originalScale;

            internal Animator animator;
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

        protected abstract class Action
        {
            protected Context context;
            protected string animationParameterName;

            protected Action(Context context) { this.context = context; }

            public abstract void Run();
        }

        protected abstract class AxisAction
        {
            protected Context context;
            protected string animationParameterName;

            protected AxisAction(Context context) { this.context = context; }

            public abstract void Run(float x, float y);
        }

        protected class Crouch : Action
        {
            [SerializeField]
            private float crouchHeight = 0.75f;
            [SerializeField]
            private float speedReduction = 0.5f;

            public Crouch(Context context, string animationParameterName) : base(context)
            {
                this.animationParameterName = animationParameterName;
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

        protected class Jump : Action
        {
            [SerializeField]
            private float jumpPower = 5.0f;

            public Jump(Context context, string animationParameterName) : base(context)
            {
                this.animationParameterName = animationParameterName;
            }

            public override void Run()
            {
                this.context.rigidbody.AddForce(0.0f, this.jumpPower, 0.0f, ForceMode.Impulse);
                this.context.isGrounded = false;
                this.context.animator.SetTrigger(this.animationParameterName);
            }
        }

        protected class Walk : AxisAction
        {
            public bool IsMovable { get; private set; }

            public Walk(Context context, string animationParameterName) : base(context)
            {
                this.IsMovable = true;
                this.animationParameterName = animationParameterName;
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

                if (x != 0 || y != 0)
                {
                    this.context.animator.SetBool(this.animationParameterName, true);
                }
                else
                {
                    this.context.animator.SetBool(this.animationParameterName, false);
                }
            }
        }

        protected class Sprint : AxisAction
        {
            public float SprintSpeed { get; private set; }

            public Sprint(Context context, string animationParameterName) : base(context)
            {
                this.SprintSpeed = 7.0f;
                this.animationParameterName = animationParameterName;
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

                if (x != 0 || y != 0)
                {
                    this.context.animator.SetBool(this.animationParameterName, true);
                }
                else
                {
                    this.context.animator.SetBool(this.animationParameterName, false);
                }
            }
        }

        [SerializeField]
        protected Rigidbody _rigidbody;

        [SerializeField]
        protected Context context;
    }
}