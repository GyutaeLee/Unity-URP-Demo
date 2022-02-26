using UnityEngine;

namespace InputSystem
{
    public class MouseInput
    {
        public Vector2 Position { get; private set; }
        public float WeightOfX { get; private set; }
        public float WeightOfY { get; private set; }

        public void Run()
        {
            this.Position = Input.mousePosition;
            this.WeightOfX = Input.GetAxis("Mouse X");
            this.WeightOfY = Input.GetAxis("Mouse Y");
        }
    }
}