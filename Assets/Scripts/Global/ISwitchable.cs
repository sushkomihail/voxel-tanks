namespace Global
{
    public interface ISwitchable
    {
        public bool IsActive { get; }
        public void Enable();
        public void Disable();
    }
}