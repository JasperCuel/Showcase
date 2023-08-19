using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cue.UI
{
    public class BannerPopup : InformationPopup
    {
        [SerializeField]
        internal Image banner;

        public void Setup(BannerPopupInfo info)
        {
            Setup(info as InformationPopupInfo);
            if (banner != null)
                banner.sprite = info.bannerSprite;
        }
    }
}