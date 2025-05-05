namespace UnityTimeline..vscode
{
    public class GameTimelineTrack_TimeScale : GameTimelineTrack
    {
        public GameTimelineTrack_TimeScale()
        {
            this.trackName = "时间缩放";
            this.trackIndex = 2;
        }
        protected override void _OnUpdate(float newTime)
        {
        }
    }

}