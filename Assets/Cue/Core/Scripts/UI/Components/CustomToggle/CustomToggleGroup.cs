using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cue.Core
{
    public class CustomToggleGroup : MonoBehaviour
    {
        [Tooltip("This toggle will be selected by default")]
        [SerializeField]
        private CustomToggle defaultToggle;

        [Tooltip("This event will be fired when a toggle in this group has been changed")]
        [SerializeField]
        private UnityEvent OnToggleChanged;

        [Header("Options")]
        [Tooltip("Makes sure there is always one value selected")]
        public bool limitToOneSelected = true;

        [Tooltip("Allows deselection")]
        [ShowIf(nameof(limitToOneSelected), true, false)]
        public bool allowDeselect = false;

        private List<CustomToggle> toggles;
        private CustomToggle currentToggle;

        public CustomToggle CurrentToggle
        { get { return currentToggle; } }

        /// <summary>
        /// Registers the toggle to the group
        /// </summary>
        /// <param name="toggle">Custom toggle</param>
        public void RegisterToggle(CustomToggle toggle)
        {
            toggles ??= new List<CustomToggle>();
            if (toggles.Contains(toggle))
                return;
            toggle.SetToggle(false, false);
            toggles.Add(toggle);
            if (toggle.Equals(defaultToggle))
                SelectToggle(toggle);
        }

        public void DeregisterToggles()
        {
            toggles = new List<CustomToggle>();
        }

        public void SelectToggleBasedOnObjectData(DataContainer data, bool triggerToggleChangedEvent = true)
        {
            foreach (CustomToggle toggle in toggles)
            {
                if (toggle.data != null && toggle.data.objectData == data.objectData)
                {
                    SelectToggle(toggle, triggerToggleChangedEvent);
                    return;
                }
            }
            ResetToggles();
        }

        /// <summary>
        /// Selects a specific toggle
        /// </summary>
        /// <param name="toggle">Toggle to select</param>
        public void SelectToggle(CustomToggle toggle, bool triggerEvents = true)
        {
            if (limitToOneSelected)
            {
                if (currentToggle == null)
                {
                    toggle.SetToggle(true, triggerEvents);
                    currentToggle = toggle;
                }
                else if (currentToggle != null && currentToggle != toggle)
                {
                    currentToggle.SetToggle(false, triggerEvents);
                    toggle.SetToggle(true, triggerEvents);
                    currentToggle = toggle;
                }
                else if (allowDeselect)
                    toggle.SetToggle(!toggle.IsOn, triggerEvents);

                currentToggle = toggle;
            }
            else
                toggle.SetToggle(!toggle.IsOn, triggerEvents);

            OnToggleChanged?.Invoke();
        }

        /// <summary>
        /// Resets all <see cref="CustomToggle"/> in <see cref="CustomToggleGroup"/>
        /// </summary>
        public void ResetToggles()
        {
            currentToggle = null;
            foreach (CustomToggle toggle in toggles)
            {
                toggle.ResetToggle();
            }
        }

        public List<CustomToggle> GetAllToggles()
        {
            return toggles;
        }

        /// <summary>
        /// Sets interactable for <see cref="Panels.SetupSettingsPanelUI"/> specifically for the amount of players selection
        /// </summary>
        /// <remarks><b>NOTE:</b> This method is not part of the base behaviour of the <see cref="CustomToggleGroup"/></remarks>
        /// <param name="amount">Current amount</param>
        public void SetInteractable(int amount)
        {
            if (toggles != null)
            {
                foreach (CustomToggle toggle in toggles)
                {
                    toggle.SetDisabled(false);
                }

                if (amount.Equals(0))
                    return;

                foreach (CustomToggle toggle in toggles)
                {
                    if (toggle.data.intData + amount > 4)
                        toggle.SetDisabled(true);
                }
            }
        }
    }
}