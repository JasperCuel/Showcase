using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cue.UI
{
    public class InformationPopup : Popup
    {
        [SerializeField]
        internal TMP_Text titleField;

        [SerializeField]
        internal TMP_Text descriptionField;

        [SerializeField]
        internal Button closeButton;

        [SerializeField]
        internal TMP_Text closeButtonField;

        public void Setup(InformationPopupInfo info)
        {
            Setup(info as PopupInfo);
            if (titleField != null)
                titleField.SetText(info.titleText);
            if (descriptionField != null)
                descriptionField.SetText(info.descriptionText);
            if (closeButtonField != null)
                closeButtonField.SetText(info.closeButtonText);
            if (closeButton != null)
                closeButton.onClick.AddListener(() => { UIManager.Instance.HideCurrentPopup(); info.onClose?.Invoke(); });
        }
    }
}