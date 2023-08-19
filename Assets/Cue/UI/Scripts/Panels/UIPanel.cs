using UnityEngine;

namespace Cue.UI
{
    [DisallowMultipleComponent]
    public class UIPanel : MonoBehaviour
    {
        [SerializeField]
        private PanelType panelId;

        public PanelType type
        { get { return panelId; } }
    }
}