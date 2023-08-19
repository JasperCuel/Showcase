using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

internal class TextToTextMeshPro : MonoBehaviour
{
    [MenuItem("Cue/Tools/Replace Text Component With Text Mesh Pro", true)]
    private static bool TextSelectedValidation()
    {
        var selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0) return false;

        foreach (var selectedObject in selectedObjects)
        {
            Text text = selectedObject.GetComponent<Text>();
            if (!text) return false;
        }

        return true;
    }

    [MenuItem("Cue/Tools/Replace Text Component With Text Mesh Pro")]
    private static void ReplaceSelectedObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        Undo.RecordObjects(selectedObjects, "Replace Text Component with Text Mesh Pro Component");

        foreach (GameObject selectedObject in selectedObjects)
        {
            Text textComp = selectedObject.GetComponent<Text>();
            Vector2 textSizeDelta = textComp.rectTransform.sizeDelta;

            //text component is still alive in memory, so the settings are still intact
            Undo.DestroyObjectImmediate(textComp);

            TextMeshProUGUI tmp = Undo.AddComponent<TextMeshProUGUI>(selectedObject);

            tmp.text = textComp.text;

            tmp.fontSize = textComp.fontSize;

            FontStyle fontStyle = textComp.fontStyle;
            switch (fontStyle)
            {
                case FontStyle.Normal:
                    tmp.fontStyle = FontStyles.Normal;
                    break;

                case FontStyle.Bold:
                    tmp.fontStyle = FontStyles.Bold;
                    break;

                case FontStyle.Italic:
                    tmp.fontStyle = FontStyles.Italic;
                    break;

                case FontStyle.BoldAndItalic:
                    tmp.fontStyle = FontStyles.Bold | FontStyles.Italic;
                    break;
            }

            tmp.enableAutoSizing = textComp.resizeTextForBestFit;
            tmp.fontSizeMin = textComp.resizeTextMinSize;
            tmp.fontSizeMax = textComp.resizeTextMaxSize;

            TextAnchor alignment = textComp.alignment;
            switch (alignment)
            {
                case TextAnchor.UpperLeft:
                    tmp.alignment = TextAlignmentOptions.TopLeft;
                    break;

                case TextAnchor.UpperCenter:
                    tmp.alignment = TextAlignmentOptions.Top;
                    break;

                case TextAnchor.UpperRight:
                    tmp.alignment = TextAlignmentOptions.TopRight;
                    break;

                case TextAnchor.MiddleLeft:
                    tmp.alignment = TextAlignmentOptions.MidlineLeft;
                    break;

                case TextAnchor.MiddleCenter:
                    tmp.alignment = TextAlignmentOptions.Midline;
                    break;

                case TextAnchor.MiddleRight:
                    tmp.alignment = TextAlignmentOptions.MidlineRight;
                    break;

                case TextAnchor.LowerLeft:
                    tmp.alignment = TextAlignmentOptions.BottomLeft;
                    break;

                case TextAnchor.LowerCenter:
                    tmp.alignment = TextAlignmentOptions.Bottom;
                    break;

                case TextAnchor.LowerRight:
                    tmp.alignment = TextAlignmentOptions.BottomRight;
                    break;
            }

            tmp.enableWordWrapping = textComp.horizontalOverflow == HorizontalWrapMode.Wrap;

            tmp.color = textComp.color;
            tmp.raycastTarget = textComp.raycastTarget;
            tmp.richText = textComp.supportRichText;

            tmp.rectTransform.sizeDelta = textSizeDelta;

            tmp.text = tmp.text.Replace("\r", "");
        }
    }
}