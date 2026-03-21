namespace QuickOutline.Scripts
{
    public class SwitchableOutline : Outline
    {
        public bool IsInteractive { get; private set; }

        public void SetIsInteractive(bool isInteractive)
        {
            IsInteractive = isInteractive;
        }
    }
}