using UnityEngine;
using UnityEngine.UI;

namespace Cue.Core
{
    [RequireComponent(typeof(Button))]
    public class Tab : MonoBehaviour
    {
        public TabGroup tabGroup;
        public GameObject tabContent;

        [HideInInspector]
        public Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            button.onClick.AddListener(SelectTab);
        }

        private void SelectTab()
        {
            tabGroup.SelectTab(this);
        }
    }
}