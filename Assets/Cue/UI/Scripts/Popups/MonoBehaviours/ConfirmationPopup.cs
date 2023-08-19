using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cue.UI
{
    public class ConfirmationPopup : InformationPopup
    {
        [SerializeField]
        internal Button confirmButton;

        [SerializeField]
        internal TMP_Text confirmButtonField;

        public void Setup(ConfirmationPopupInfo info)
        {
            Setup(info as InformationPopupInfo);
            if (confirmButtonField != null)
                confirmButtonField.SetText(info.confirmButtonText);
            if (confirmButton != null)
                confirmButton.onClick.AddListener(() => { UIManager.Instance.HideCurrentPopup(); info.onConfirm?.Invoke(); });
        }
    }
}