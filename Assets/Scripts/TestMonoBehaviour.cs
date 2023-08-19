using UnityEngine;

namespace Project
{
    public class TestMonoBehaviour : MonoBehaviour
    {
        [SerializeField]
        private string text;
        [SerializeField]
        private DebugType type;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DebugLogger.Log(text, type);
            }
        }
    }
}