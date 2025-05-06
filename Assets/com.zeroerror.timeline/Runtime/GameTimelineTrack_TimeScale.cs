namespace ZeroError.Config
{

    public class GameTimelineTrack_TimeScale : GameTimelineTrack_Skill
    {
        public GameTimelineTrack_TimeScale(GameTimeline_Skill skillTimeline) : base(skillTimeline)
        {
            this.trackName = "时间缩放";
            this.trackIndex = 2;
        }
    }
}