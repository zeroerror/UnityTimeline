using UnityEditor;
using UnityEngine;
using Game.Config;

namespace Game.GameEditor
{
    [CustomPropertyDrawer(typeof(GameTimelineTrack_Loop))]
    public class GameTimelineTrackPropertyDrawer_Loop : GameTimelineTrackPropertyDrawer
    {
        protected override Color fragmentColor => new Color(0.5f, 1f, 0.5f, 0.5f);
        protected override void OnTimeUpdate(SerializedProperty property, float time, float lastTime)
        {
        }
    }
}