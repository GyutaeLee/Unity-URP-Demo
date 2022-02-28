using UnityEngine;
using System.Collections.Generic;

using Demo.Game.Controller.Player;

namespace Demo.Game.Input.Player
{
    public sealed class PlayerInput : MonoBehaviour
    {
        [SerializeField]
        private PlayerCameraController cameraController;
        [SerializeField]
        private PlayerController playerController;

        private TwoAxisAction walk;
        private TwoAxisAction _camera;

        private BasicKeyAction jump;
        private BasicKeyAction sprint;
        private BasicKeyAction crouch;

        private List<IActionProvider> actionsProviders;

        private void Awake()
        {
            this.walk = new TwoAxisAction("Horizontal", "Vertical", 0.0f);
            this._camera = new TwoAxisAction("Mouse X", "Mouse Y", 0.0f);

            this.jump = new BasicKeyAction("Jump");
            this.sprint = new BasicKeyAction("Sprint");
            this.crouch = new BasicKeyAction("Crouch");

            this.actionsProviders = new List<IActionProvider>()
            {
                this.walk,
                this.jump,
                this.sprint,
                this.crouch,
                this._camera,
            };
        }

        private void Update()
        {
            foreach (var actionProvider in this.actionsProviders)
            {
                actionProvider.Run();
            }

            float x = this._camera.horizontalValue;
            float y = this._camera.verticalValue;

            this.cameraController.UpdateRotation(x, y);

            if (this.jump.IsPressed == true)
            {
                this.playerController.DoJump();
            }

            if (this.crouch.IsPressed == true)
            {
                this.playerController.DoCrouch();
            }
        }

        private void FixedUpdate()
        {
            float x = this.walk.horizontalValue;
            float y = this.walk.verticalValue;

            if (this.sprint.WasPressed == true)
            {
                this.playerController.DoSprint(x, y);
            }
            else
            {
                this.playerController.DoWalk(x, y);
            }
        }
    }
}
