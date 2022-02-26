namespace InputSystem
{
    public class AxisInput
    {
        private KeyInput negativeKeyInput;
        private KeyInput positiveKeyInput;

        public float Value
        {
            get
            {
                if (this.negativeKeyInput.IsKeyPressed == true
                    && this.positiveKeyInput.IsKeyPressed == false)
                {
                    return -1.0f;
                }
                else if (this.negativeKeyInput.IsKeyPressed == false
                    && this.positiveKeyInput.IsKeyPressed == true)
                {
                    return 1.0f;
                }
                else
                {
                    return 0.0f;
                }
            }
        }

        public AxisInput(KeyInput negativeKeyInput, KeyInput positiveKeyInput)
        {
            this.negativeKeyInput = negativeKeyInput;
            this.positiveKeyInput = positiveKeyInput;
        }
    }
}