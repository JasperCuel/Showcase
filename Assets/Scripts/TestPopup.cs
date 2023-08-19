using Cue.UI;
using UnityEngine;

namespace Project
{
    public class TestPopup : MonoBehaviour
    {
        public Sprite customBanner;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                UIManager.Instance.QueuePopup(PopupPrefabs.GetInformationPopup());
            if (Input.GetKeyDown(KeyCode.C))
                UIManager.Instance.QueuePopup(PopupPrefabs.GetConfirmationPopup());
            if (Input.GetKeyDown(KeyCode.B))
                UIManager.Instance.QueuePopup(PopupPrefabs.GetBannerPopup(customBanner));
            if (Input.GetKeyDown(KeyCode.U))
                UIManager.Instance.QueuePopup(new UniquePopupInfo(PopupPrefabs.ExampleUniquePopup));
        }
    }
}