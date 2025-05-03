using UnityEngine;
using UnityEditor;


[System.Serializable]
public class MyCustomAttribute { }

// 使用 PropertyDrawer
[CustomPropertyDrawer(typeof(MyCustomAttribute))]
public class MyCustomPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

    }
}