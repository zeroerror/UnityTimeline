using UnityEngine;

namespace Game.Config
{
    public class GameTimelineTrack_Skill : GameTimelineTrack
    {
        public GameTimeline_Skill skillTimeline;
        public GameTimelineTrack_Skill(GameTimeline_Skill skillTimeline)
        {
            this.skillTimeline = skillTimeline;
        }
    }
}