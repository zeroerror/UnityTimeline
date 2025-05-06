using UnityEngine;

namespace ZeroError.Config
{
    public class GameTimelineTrack_Loop : GameTimelineTrack_Skill
    {
        public GameTimelineTrack_Loop(GameTimeline_Skill skillTimeline) : base(skillTimeline)
        {
            this.trackName = "时间循环";
            this.trackIndex = 3;
        }
    }
}