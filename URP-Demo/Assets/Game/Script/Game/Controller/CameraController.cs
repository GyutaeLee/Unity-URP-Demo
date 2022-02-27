using UnityEngine;

namespace Demo.Game.Controller
{
    public sealed class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform charcterBody;

        [SerializeField]
        private float mouseSensitivity = 100.0f;

        [SerializeField]
        private float maxLookAngleLimit = 50.0f;

        private float yaw = 0.0f;
        private float pitch = 0.0f;

        public void UpdateRotation(float mouseX, float mouseY)
        {
            this.yaw = this.charcterBody.transform.localEulerAngles.y + mouseX * this.mouseSensitivity;
            this.pitch -= this.mouseSensitivity * mouseY;
            this.pitch = Mathf.Clamp(this.pitch, -this.maxLookAngleLimit, this.maxLookAngleLimit);

            this.charcterBody.transform.localEulerAngles = new Vector3(0, this.yaw, 0);
            this.transform.localEulerAngles = new Vector3(this.pitch, 0, 0);
        }
    }
}
