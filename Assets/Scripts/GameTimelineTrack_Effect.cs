using UnityEngine;

namespace Game.Config
{
    public class GameTimelineTrack_Effect : GameTimelineTrack_Skill
    {
        public GameTimelineTrack_Effect(GameTimeline_Skill skillTimeline) : base(skillTimeline)
        {
            this.trackName = "特效";
            this.trackIndex = 1;
        }
    }
}