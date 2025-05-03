using UnityEditor;
using UnityEngine;

namespace Game.GameEditor
{
    public abstract class GamePropertyDrawer : PropertyDrawer
    {
        protected GameEditorLayout _editorLayout = new();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            this._editorLayout.SetAnchorPos(rect.position);
            _OnGUI(rect, property, label);
            EditorGUI.EndProperty();
        }

        protected abstract void _OnGUI(Rect position, SerializedProperty property, GUIContent label);
    }
}