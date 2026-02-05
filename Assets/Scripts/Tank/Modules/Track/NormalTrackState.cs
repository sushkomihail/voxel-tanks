namespace Tank.Modules.Track
{
    public class NormalTrackState : TrackState
    {
        private const float NormalTorqueRate = 1;
        
        public NormalTrackState(Track track) : base(track)
        {
        }

        public override void Enter()
        {
            _track.SetTorqueRate(NormalTorqueRate);
        }
    }
}