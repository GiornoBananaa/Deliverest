using UnityEditor;
using UnityEngine;

namespace Tutorial
{
    [CustomPropertyDrawer(typeof(ArrayElementTitleAttribute))]
    public class ArrayElementTitleAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
       
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty nameProp = property.serializedObject.FindProperty(property.propertyPath);
            if (nameProp != null && nameProp.managedReferenceValue != null)
                label = new GUIContent(label) { text = GetTitle(nameProp) };
           
            EditorGUI.PropertyField(position, property, label, true);
        }
       
        private string GetTitle(SerializedProperty prop)
        {
            return prop.managedReferenceValue.ToString();
        }
    }
}