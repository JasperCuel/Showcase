using Cue.Core;
using UnityEngine.SceneManagement;

namespace Cue.UI
{
    public static class CustomSceneManager
    {
        public static void LoadScene(SceneReference scene)
        {
            try
            {
                SceneManager.LoadScene(scene);
            }
            catch
            {
                DebugLogger.LogError($"Loading scene failed. Scene with name {scene} doesn't exist.");
            }
        }
    }
}