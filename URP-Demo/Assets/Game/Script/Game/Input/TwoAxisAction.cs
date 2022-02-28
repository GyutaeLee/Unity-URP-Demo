using UnityEngine;

namespace Demo.Game.Input
{
    public sealed class TwoAxisAction : IActionProvider
    {
        private string horizontalKey;
        private string verticalKey;

        private float stateThreshold;

        public TwoAxisAction(string horizontalKey, string verticalKey, float stateThreshold)
        {
            this.horizontalKey = horizontalKey;
            this.verticalKey = verticalKey;

            this.stateThreshold = stateThreshold;
        }

        public float horizontalValue { get; private set; }
        public float verticalValue { get; private set; }

        public void Run()
        {
            float horizontalAxis = UnityEngine.Input.GetAxis(this.horizontalKey);
            float verticalAxis = UnityEngine.Input.GetAxis(this.verticalKey);

            if (Mathf.Abs(horizontalAxis) > this.stateThreshold)
            {
                this.horizontalValue = horizontalAxis;
            }
            else
            {
                this.horizontalValue = 0.0f;
            }

            if (Mathf.Abs(verticalAxis) > this.stateThreshold)
            {
                this.verticalValue = verticalAxis;
            }
            else
            {
                this.verticalValue = 0.0f;
            }
        }
    }
}
