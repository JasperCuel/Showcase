using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Object))]
public class EnumEditorWindow : Editor
{
    [MenuItem("Cue/Window/EnumEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EnumEditor));
    }
}

public class EnumEditor : EditorWindow
{
    private Object obj;

    private List<string> enumValues;
    private string filePath;
    private string fileContents;
    private string beforeEnum;
    private string afterEnum;

    private int enumStartIndex;
    private int enumEndIndex;

    private string addField;

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        Object newObj = EditorGUILayout.ObjectField(obj, typeof(MonoScript), false);

        if (newObj != null && (newObj != obj || GUILayout.Button(EditorGUIUtility.IconContent("refresh"), GUILayout.Width(37))))
        {
            obj = newObj;

            filePath = AssetDatabase.GetAssetPath(obj);
            fileContents = File.ReadAllText(filePath);
            int enumIndex = fileContents.IndexOf(" enum ");
            if (enumIndex > 0)
            {
                string sub = fileContents[enumIndex..];
                string[] split = sub.Split('{', '}');
                enumValues = split.ToList();

                int bracketIndex = sub.IndexOf('{');
                enumStartIndex = enumIndex + bracketIndex;
                beforeEnum = fileContents[..enumStartIndex];

                int enumLengthIndex = fileContents[(enumStartIndex + 1)..].IndexOf('}');
                enumEndIndex = enumStartIndex + enumLengthIndex;
                afterEnum = fileContents[enumEndIndex..];
            }
            else
            {
                obj = null;
                enumValues?.Clear();
                EditorGUILayout.EndHorizontal();
                return;
            }

            for (int i = 2; i < enumValues.Count; i++)
            {
                afterEnum += enumValues[i];
            }

            string values = enumValues[1];
            string[] seperateValues = values.Split(',');
            enumValues = new List<string>();
            foreach (string value in seperateValues)
            {
                enumValues.Add(value.Trim());
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (enumValues != null)
            for (int i = 0; i < enumValues.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(i.ToString(), GUILayout.Width(15));
                GUILayout.TextField(enumValues[i]);
                if (GUILayout.Button("Remove", GUILayout.Width(75)))
                {
                    enumValues.Remove(enumValues[i]);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }

        GUILayout.FlexibleSpace();

        addField = GUILayout.TextField(addField);
        if (GUILayout.Button("Add new") && !string.IsNullOrEmpty(addField))
        {
            enumValues.Add(addField);
            addField = "";
        }

        GUILayout.Space(25);

        if (GUILayout.Button("Save to file"))
        {
            string data = GetUpdatedFile();
            Save(data);
            AssetDatabase.ImportAsset(filePath);
            Debug.Log($"Saved updated file to: {filePath}");
        }
    }

    private string GetUpdatedFile()
    {
        string newData = "";
        newData += $"{beforeEnum}{@"{"}";
        for (int i = 0; i < enumValues.Count; i++)
        {
            newData += $"\n\t\t{enumValues[i]},";
            if (i == enumValues.Count - 1)
                newData = newData.TrimEnd(',');
        }
        newData += $"\n\t";
        newData = newData.TrimEnd(' ');
        newData += afterEnum;
        newData = newData.Replace("\n\r", "\n").Replace("\r", "");
        newData = newData.Trim();
        return newData;
    }

    private void Save(string data)
    {
        File.WriteAllText(filePath, data);
    }
}