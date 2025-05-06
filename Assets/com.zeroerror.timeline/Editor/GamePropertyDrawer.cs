using UnityEditor;
using UnityEngine;

namespace Game.GameEditor
{
    public abstract class GamePropertyDrawer : PropertyDrawer
    {
        protected GameEditorLayout _editorLayout = new();
        protected float deltaTime { get; private set; }
        private float _lastTime;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var singleLineHeight = EditorGUIUtility.singleLineHeight;
            return singleLineHeight;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (_lastTime == 0)
            {
                _lastTime = Time.realtimeSinceStartup;
            }
            else
            {
                deltaTime = Time.realtimeSinceStartup - _lastTime;
                _lastTime = Time.realtimeSinceStartup;
            }
            EditorGUI.BeginProperty(rect, label, property);
            this._editorLayout.SetAnchorPos(rect.position);
            _OnGUI(rect, property, label);
            EditorGUI.EndProperty();
        }

        protected abstract void _OnGUI(Rect position, SerializedProperty property, GUIContent label);
    }
}