using Cue.Core;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfNotAttribute))]
public class ShowIfNotDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfNotAttribute showIfNot = attribute as ShowIfNotAttribute;
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(showIfNot.conditionField);
        if (sourcePropertyValue == null)
            return;

        bool condition = CheckCondition(sourcePropertyValue, showIfNot);

        if (condition)
            EditorGUI.PropertyField(position, property, label, true);
        else
        {
            if (!showIfNot.hide)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = true;
            }

            if (showIfNot.defaultValue == null)
                return;

            if (property.propertyType == SerializedPropertyType.Integer)
                property.intValue = (int)showIfNot.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Boolean)
                property.boolValue = (bool)showIfNot.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Float)
                property.floatValue = (float)showIfNot.defaultValue;
            else if (property.propertyType == SerializedPropertyType.String)
                property.stringValue = (string)showIfNot.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Color)
                property.colorValue = (Color)showIfNot.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Enum)
                property.enumValueIndex = (int)showIfNot.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Vector2)
                property.vector2Value = (Vector2)showIfNot.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Vector3)
                property.vector2Value = (Vector3)showIfNot.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Vector4)
                property.vector4Value = (Vector4)showIfNot.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Quaternion)
                property.quaternionValue = (Quaternion)showIfNot.defaultValue;
        }
    }

    private bool CheckCondition(SerializedProperty sourcePropertyValue, ShowIfNotAttribute showIfNot) => sourcePropertyValue.propertyType switch
    {
        SerializedPropertyType.Integer => showIfNot.hasToEqualValues.Any(x => (int)x == sourcePropertyValue.intValue),
        SerializedPropertyType.Boolean => showIfNot.hasToEqualValues.Any(x => (bool)x == sourcePropertyValue.boolValue),
        SerializedPropertyType.Float => showIfNot.hasToEqualValues.Any(x => (float)x == sourcePropertyValue.floatValue),
        SerializedPropertyType.String => showIfNot.hasToEqualValues.Any(x => (string)x == sourcePropertyValue.stringValue),
        SerializedPropertyType.Color => showIfNot.hasToEqualValues.Any(x => (Color)x == sourcePropertyValue.colorValue),
        SerializedPropertyType.Enum => showIfNot.hasToEqualValues.Any(x => (int)x == sourcePropertyValue.enumValueIndex),
        SerializedPropertyType.Vector2 => showIfNot.hasToEqualValues.Any(x => (Vector2)x == sourcePropertyValue.vector2Value),
        SerializedPropertyType.Vector3 => showIfNot.hasToEqualValues.Any(x => (Vector3)x == sourcePropertyValue.vector3Value),
        SerializedPropertyType.Vector4 => showIfNot.hasToEqualValues.Any(x => (Vector4)x == sourcePropertyValue.vector4Value),
        SerializedPropertyType.Quaternion => showIfNot.hasToEqualValues.Any(x => (Quaternion)x == sourcePropertyValue.quaternionValue),
        _ => false
    };

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property, label, true);
}