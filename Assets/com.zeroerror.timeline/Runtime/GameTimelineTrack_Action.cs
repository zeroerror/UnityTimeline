using UnityEngine;

namespace Game.Config
{
    public class GameTimelineTrack_Action : GameTimelineTrack_Skill
    {
        public GameTimelineTrack_Action(GameTimeline_Skill skillTimeline) : base(skillTimeline)
        {
            this.trackName = "动作";
            this.trackIndex = 0;
        }
    }
}