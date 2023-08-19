using UnityEngine;

namespace Cue.Core
{
    /// <summary>
    /// Handles singleton of type T
    /// </summary>
    /// <typeparam name="T">Type to create a singleton of</typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        /// <summary>
        /// An instance of type <typeparamref name="T"/>
        /// </summary>
        public static T Instance
        {
            get
            {
                if(_instance == null)
                {
                    T[] objects = FindObjectsOfType(typeof(T)) as T[];
                    if(objects.Length > 0)
                        _instance = objects[0];
                    if (objects.Length > 1)
                    {
                        int destroyedAmount = 0;
                        for (int i = 1; i < objects.Length; i++)
                        {
                            Destroy(objects[i].gameObject);
                            destroyedAmount++;
                        }
                        DebugLogger.LogWarning($"There is more than one {typeof(T).Name} in the scene! Destroyed {destroyedAmount} instances.", DebugType.Info);
                    }
                    if(_instance == null)
                    {
                        GameObject obj = new GameObject { hideFlags = HideFlags.HideAndDontSave };
                        _instance = obj.AddComponent<T>();
                        DebugLogger.LogWarning($"There is no {typeof(T).Name} in the scene. Created a new default {typeof(T).Name}", DebugType.Info);
                    }
                }
                return _instance;
            }
        }
    }
}