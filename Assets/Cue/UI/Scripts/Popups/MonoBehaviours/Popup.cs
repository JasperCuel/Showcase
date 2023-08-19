using UnityEngine;

namespace Cue.UI
{
    [DisallowMultipleComponent]
    public class Popup : MonoBehaviour
    {
        public PopupType type;

        public void Setup(PopupInfo info)
        {
            type = info.type;
        }
    }
}