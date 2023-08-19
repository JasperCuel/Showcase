using UnityEditor;
using UnityEngine;

internal class RemoveMonoBehavioursWithMissingScript : MonoBehaviour
{
    private static int totalReferencesRemoved = 0;
    private static int gameObjectsChecked = 0;

    [MenuItem("Cue/Tools/Remove missing mono behaviour script references", true)]
    private static bool TextSelectedValidation()
    {
        var selectedObjects = Selection.gameObjects;
        return selectedObjects.Length > 0;
    }

    [MenuItem("Cue/Tools/Remove missing mono behaviour script references")]
    private static void RemoveMissingScripts()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        Undo.RecordObjects(selectedObjects, "Remove missing mono behaviour scripts from selected objects");

        totalReferencesRemoved = 0;
        gameObjectsChecked = 0;
        foreach (GameObject selectedObject in selectedObjects)
        {
            RemoveRecursive(selectedObject);
        }
        Debug.Log($"Removed {totalReferencesRemoved} missing references on {gameObjectsChecked} game objects");
    }

    private static void RemoveRecursive(GameObject selectedObject)
    {
        totalReferencesRemoved += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(selectedObject);
        gameObjectsChecked++;
        foreach (Transform child in selectedObject.transform)
        {
            RemoveRecursive(child.gameObject);
        }
    }
}