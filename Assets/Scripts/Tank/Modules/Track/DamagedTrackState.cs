namespace Tank.Modules.Track
{
    public class DamagedTrackState : TrackState
    {
        public DamagedTrackState(Track track) : base(track)
        {
        }

        public override void Enter()
        {
            _track.SetTorqueRate(_track.DamagedTorqueRate);
        }
    }
}