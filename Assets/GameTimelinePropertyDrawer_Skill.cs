using UnityEditor;
using UnityEngine;

namespace Game.GameEditor
{
    [CustomPropertyDrawer(typeof(GameTimeline_Skill))]
    public class GameTimelinePropertyDrawer_Skill : GameTimelinePropertyDrawer
    {
        float extraHeight = EditorGUIUtility.singleLineHeight * 2;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + extraHeight;
        }

        protected override void _OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var animClip_p = property.FindPropertyRelative("animClip");
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, animClip_p, new GUIContent("动画"));
            rect.y += EditorGUIUtility.singleLineHeight;

            var animGO_p = property.FindPropertyRelative("animGO");
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, animGO_p, new GUIContent("绑定物体"));
            rect.y += EditorGUIUtility.singleLineHeight;

            this._editorLayout.SetAnchorPos(rect.position);
            base._OnGUI(rect, property, label);
        }
        protected override void _OnTimelineUpdate(SerializedProperty property, float time)
        {
            var animClip_p = property.FindPropertyRelative("animClip");
            var animGO_p = property.FindPropertyRelative("animGO");
            var animClip = animClip_p.objectReferenceValue as AnimationClip;
            var animGO = animGO_p.objectReferenceValue as GameObject;
            if (animClip == null || animGO == null)
            {
                return;
            }
            animClip.SampleAnimation(null, time);
        }
    }
}
