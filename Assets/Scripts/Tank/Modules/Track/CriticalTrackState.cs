namespace Tank.Modules.Track
{
    public class CriticalTrackState : TrackState
    {
        private const float CriticalTorqueRate = 0;
        
        public CriticalTrackState(Track track) : base(track)
        {
        }

        public override void Enter()
        {
            _track.SetTorqueRate(CriticalTorqueRate);
        }
    }
}