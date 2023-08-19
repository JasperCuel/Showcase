using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cue.Core
{
    public class TabGroup : MonoBehaviour
    {
        private List<Tab> tabs;

        [Header("Colours")]
        [SerializeField]
        private Color selectedColour = Color.white;
        [SerializeField]
        private Color idleColour = Color.white;

        private void Start()
        {
            tabs = FindObjectsOfType<Tab>().Where(x => x.tabGroup == this).ToList();
        }

        public void SelectTab(Tab selectedTab)
        {
            if (!tabs.Contains(selectedTab))
                return;

            foreach (Tab tab in tabs)
            {
                bool activate = tab == selectedTab;
                tab.tabContent.SetActive(activate);
                tab.button.image.color = activate ? selectedColour : idleColour;
            }
        }
    }
}