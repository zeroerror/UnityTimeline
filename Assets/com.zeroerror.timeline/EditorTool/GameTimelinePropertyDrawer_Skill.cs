using UnityEditor;
using UnityEngine;
using ZeroError.Config;
using UnityEngine.SceneManagement;
using System.Linq;

namespace ZeroError.EditorTool
{
    [CustomPropertyDrawer(typeof(GameTimeline_Skill))]
    public class GameTimelinePropertyDrawer_Skill : GameTimelinePropertyDrawer
    {
        float extraHeight = EditorGUIUtility.singleLineHeight * 1;
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
            this._editorLayout.SetAnchorPos(rect.position);
            base._OnGUI(rect, property, label);
        }
        protected override void _OnTimelineUpdate(SerializedProperty property, float time)
        {
            var animClip_p = property.FindPropertyRelative("animClip");
            var animClip = animClip_p.objectReferenceValue as AnimationClip;
            if (animClip == null)
            {
                return;
            }
            var displayHuman = SceneManager.GetActiveScene().GetRootGameObjects().ToList().Find(x => x.name == "DisplayHuman");
            if (displayHuman == null)
            {
                displayHuman = GameObject.Instantiate(Resources.Load<GameObject>("DisplayHuman"));
                displayHuman.name = "DisplayHuman";
                displayHuman.transform.position = Vector3.zero;
                displayHuman.transform.rotation = Quaternion.identity;
                displayHuman.transform.localScale = Vector3.one;
                Debug.Log("展示人形不存在，已自动创建");
            }
            animClip.SampleAnimation(displayHuman, time);
        }
    }
}
