using UnityEditor;
using UnityEngine;

namespace Game.GameEditor
{
    [CustomPropertyDrawer(typeof(GameTimelineTrack_Effect))]
    public class GameTimelineTrackPropertyDrawer_Effect : GameTimelineTrackPropertyDrawer
    {
        protected override Color fragmentColor => new Color(0.5f, 0.5f, 1f, 0.5f);
        protected override void OnTimeUpdate(SerializedProperty property, float time, float lastTime)
        {
        }
    }
}