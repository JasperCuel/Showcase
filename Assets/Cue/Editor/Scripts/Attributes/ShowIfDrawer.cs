using Cue.Core;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = attribute as ShowIfAttribute;
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(showIf.conditionField);

        bool condition = CheckCondition(sourcePropertyValue, showIf);

        if (condition)
            EditorGUI.PropertyField(position, property, label, true);
        else
        {
            if (!showIf.hide)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = true;
            }

            if (showIf.defaultValue == null)
                return;

            if (property.propertyType == SerializedPropertyType.ObjectReference)
                property.objectReferenceValue = (Object)showIf.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Integer)
                property.intValue = (int)showIf.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Boolean)
                property.boolValue = (bool)showIf.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Float)
                property.floatValue = (float)showIf.defaultValue;
            else if (property.propertyType == SerializedPropertyType.String)
                property.stringValue = (string)showIf.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Color)
                property.colorValue = (Color)showIf.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Enum)
                property.enumValueIndex = (int)showIf.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Vector2)
                property.vector2Value = (Vector2)showIf.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Vector3)
                property.vector2Value = (Vector3)showIf.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Vector4)
                property.vector4Value = (Vector4)showIf.defaultValue;
            else if (property.propertyType == SerializedPropertyType.Quaternion)
                property.quaternionValue = (Quaternion)showIf.defaultValue;
        }
    }

    private bool CheckCondition(SerializedProperty sourcePropertyValue, ShowIfAttribute showIf)
    {
        if ((showIf.hasToEqualValues == null || showIf.hasToEqualValues[0] == null) && sourcePropertyValue == null)
            return true;
        else if (showIf.hasToEqualValues[0] is int && sourcePropertyValue.propertyType == SerializedPropertyType.Integer)
            return showIf.hasToEqualValues.Any(x => (int)x == sourcePropertyValue.intValue);
        else if (showIf.hasToEqualValues[0] is bool && sourcePropertyValue.propertyType == SerializedPropertyType.Boolean)
            return showIf.hasToEqualValues.Any(x => (bool)x == sourcePropertyValue.boolValue);
        else if (showIf.hasToEqualValues[0] is float && sourcePropertyValue.propertyType == SerializedPropertyType.Float)
            return showIf.hasToEqualValues.Any(x => (float)x == sourcePropertyValue.floatValue);
        else if (showIf.hasToEqualValues[0] is string && sourcePropertyValue.propertyType == SerializedPropertyType.String)
            return showIf.hasToEqualValues.Any(x => (string)x == sourcePropertyValue.stringValue);
        else if (showIf.hasToEqualValues[0] is Color && sourcePropertyValue.propertyType == SerializedPropertyType.Color)
            return showIf.hasToEqualValues.Any(x => (Color)x == sourcePropertyValue.colorValue);
        else if (showIf.hasToEqualValues[0] is System.Enum && sourcePropertyValue.propertyType == SerializedPropertyType.Enum)
            return showIf.hasToEqualValues.Any(x => (int)x == sourcePropertyValue.enumValueIndex);
        else if (showIf.hasToEqualValues[0] is Vector2 && sourcePropertyValue.propertyType == SerializedPropertyType.Vector2)
            return showIf.hasToEqualValues.Any(x => (Vector2)x == sourcePropertyValue.vector2Value);
        else if (showIf.hasToEqualValues[0] is Vector3 && sourcePropertyValue.propertyType == SerializedPropertyType.Vector3)
            return showIf.hasToEqualValues.Any(x => (Vector3)x == sourcePropertyValue.vector3Value);
        else if (showIf.hasToEqualValues[0] is Vector4 && sourcePropertyValue.propertyType == SerializedPropertyType.Vector4)
            return showIf.hasToEqualValues.Any(x => (Vector4)x == sourcePropertyValue.vector4Value);
        else if (showIf.hasToEqualValues[0] is Quaternion && sourcePropertyValue.propertyType == SerializedPropertyType.Quaternion)
            return showIf.hasToEqualValues.Any(x => (Quaternion)x == sourcePropertyValue.quaternionValue);
        else
            return false;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property, label, true);
}