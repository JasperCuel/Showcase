using UnityEngine;

namespace Cue.UI
{
    public static class PopupPrefabs
    {
        public const string ExampleUniquePopup = "ExampleUniquePopup";

        public static InformationPopupInfo GetInformationPopup() => new InformationPopupInfo(PopupType.Information,
            "Information Popup",
            "This is an information popup. Use it wisely!",
            "Thank you!",
            null);

        public static ConfirmationPopupInfo GetConfirmationPopup() => new ConfirmationPopupInfo(PopupType.Confirmation,
            "Are you sure?",
            "This is a confirmation popup. Do you wish to proceed at your own risk? It'll show you an unique popup.",
            "No thanks",
            null,
            "Of course!",
            () => UIManager.Instance.QueuePopup(new UniquePopupInfo(ExampleUniquePopup), true));

        public static BannerPopupInfo GetBannerPopup(Sprite scoreHintBannerSprite) => new BannerPopupInfo(PopupType.Banner,
            "This is a custom banner.",
            "I made this especially for you. Do you like it? You could close this window but it'll be gone forever!",
            scoreHintBannerSprite,
            "Its ugly!",
            null);
    }
}