namespace Tank.Modules.Track
{
    public abstract class TrackState
    {
        protected Track _track;

        protected TrackState(Track track)
        {
            _track = track;
        }
        
        public abstract void Enter();
    }
}