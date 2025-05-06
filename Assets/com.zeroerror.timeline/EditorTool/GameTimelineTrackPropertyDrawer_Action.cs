using UnityEditor;
using UnityEngine;
using ZeroError.Config;

namespace ZeroError.EditorTool
{
    [CustomPropertyDrawer(typeof(GameTimelineTrack_Action))]
    public class GameTimelineTrackPropertyDrawer_Action : GameTimelineTrackPropertyDrawer
    {
        protected override Color fragmentColor => new Color(1f, 0.5f, 0.5f, 0.5f);
        protected override void OnTimeUpdate(SerializedProperty property, float time, float lastTime)
        {
        }
    }
}