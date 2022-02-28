namespace Demo.Game.Input
{
    public sealed class BasicKeyAction : IActionProvider
    {
        public bool IsPressed { get; private set; }
        public bool WasPressed { get; private set; }
        public bool WasReleased { get; private set; }

        private string key;

        public BasicKeyAction(string key)
        {
            this.key = key;
        }

        public void Run()
        {
            bool isStay = UnityEngine.Input.GetButton(key);
            bool isEnter = UnityEngine.Input.GetButtonDown(key);
            bool isRelease = UnityEngine.Input.GetButtonUp(key);

            if (isEnter == true && this.IsPressed == false)
            {
                this.IsPressed = true;
                this.WasReleased = false;
            }
            else if (this.IsPressed == true && isStay == true)
            {
                this.WasPressed = true;
                this.IsPressed = false;
            }

            if (isRelease == true)
            {
                this.WasPressed = false;
                this.WasReleased = true;
            }
        }
    }
}
