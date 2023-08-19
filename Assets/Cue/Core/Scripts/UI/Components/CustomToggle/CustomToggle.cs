using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cue.Core
{
    public class CustomToggle : ButtonBehaviour
    {
        [Tooltip("The current state of the toggle")]
        public bool IsOn { get; set; }

        [Tooltip("A reference to the toggle group this toggle is part of")]
        [SerializeField]
        public CustomToggleGroup toggleGroup;

        [Tooltip("Is this toggle currently disabled? It won't be able to be selected")]
        [SerializeField]
        private bool isDisabled = false;

        [Tooltip("This is a simple data container to store data within the toggle")]
        public DataContainer data;

        [Tooltip("The color used when the toggle is disabled")]
        [SerializeField]
        private Color disabledColor = Color.white;

        [Header("Optional")]
        [SerializeField]
        private TMP_Text textField;

        [SerializeField]
        private Image image;

        [Header("Colors")]
        [Tooltip("Should the image colour change based on if the toggle is currently selected")]
        [SerializeField]
        private bool toggleImageColor = true;

        [Tooltip("The colour used when the toggle is selected")]
        [ShowIf(nameof(toggleImageColor), true, default)]
        [SerializeField]
        private Color selectedColor = Color.white;

        [Tooltip("The colour used when the toggle is not selected")]
        [ShowIf(nameof(toggleImageColor), true, default)]
        [SerializeField]
        private Color notSelectedColor = Color.white;

        [Header("Checkmark")]
        [Tooltip("Should a checkmark be enabled and disabled based on if the toggle is currently selected")]
        [SerializeField]
        private bool useCheckmark = false;

        [ShowIf(nameof(useCheckmark), true, default)]
        [SerializeField]
        private Image checkmark;

        private Action<DataContainer> onSelected;
        private Action<DataContainer> onDeselected;

        private ScrollRect scrollRect;

        private void Start()
        {
            if (image == null)
                image = GetComponent<Image>();
            if (scrollRect == null)
                scrollRect = GetComponentInParent<ScrollRect>();
            toggleGroup.RegisterToggle(this);
        }

        /// <summary>
        /// Sets up the custom toggle
        /// </summary>
        /// <param name="toggleGroup">Toggle group this toggle is part of</param>
        /// <param name="text">(optional) Text that should be displayed next to the toggle</param>
        /// <param name="data">(optional) A data container to store information within the button. This will be used in the <see cref="onSelected"/> and <see cref="onDeselected"/> actions</param>
        /// <param name="onSelected">(optional) Action performed when the toggle gets selected</param>
        /// <param name="onDeselected">(optional) Action performed when the toggle gets deselected</param>
        public void Setup(CustomToggleGroup toggleGroup, string text = "", DataContainer data = null, Action<DataContainer> onSelected = null, Action<DataContainer> onDeselected = null)
        {
            this.toggleGroup = toggleGroup;
            if (textField)
                textField.SetText(text);
            if (data != null)
                this.data = data;
            if (onSelected != null)
                this.onSelected = onSelected;
            if (onDeselected != null)
                this.onDeselected = onDeselected;
        }

        public override void OnClick()
        {
            if (isDisabled)
                return;
            toggleGroup.SelectToggle(this);
        }

        /// <summary>
        /// Sets the toggle value
        /// </summary>
        /// <param name="isOn">Should the toggle be turned on</param>
        /// <param name="triggerEvents">Is this toggle being initialised? (doesn't call <see cref="onSelected"/> and <see cref="onDeselected"/> actions</param>
        public void SetToggle(bool isOn, bool triggerEvents = true)
        {
            if (this.IsOn == isOn && triggerEvents)
                return;
            if (!image)
                Start();
            this.IsOn = isOn;
            if (data != null)
                data.isOn = isOn;

            if (toggleImageColor)
                image.color = isOn ? selectedColor : notSelectedColor;
            if (useCheckmark && checkmark != null)
                checkmark.gameObject.SetActive(isOn);
            if (isOn && triggerEvents && onSelected != null)
                onSelected?.Invoke(data);
            if (!isOn && triggerEvents && onDeselected != null)
                onDeselected?.Invoke(data);
        }

        /// <summary>
        /// Resets toggle to deselected state
        /// </summary>
        public void ResetToggle()
        {
            if (!image)
                Start();

            IsOn = false;
            data.isOn = false;

            if (toggleImageColor)
                image.color = notSelectedColor;
            if (useCheckmark && checkmark != null)
                checkmark.gameObject.SetActive(false);
        }

        /// <summary>
        /// Set disabled state for toggle
        /// </summary>
        /// <param name="isDisabled">Disables button if true and enables it if false</param>
        public void SetDisabled(bool isDisabled)
        {
            this.isDisabled = isDisabled;
            image.color = isDisabled ? disabledColor : (IsOn ? selectedColor : notSelectedColor);
        }
    }
}