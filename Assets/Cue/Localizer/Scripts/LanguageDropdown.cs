using TMPro;
using UnityEngine;

namespace Cue.Localizer
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LanguageDropdown : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text dropdownCaption;

        private TMP_Dropdown dropdown;

        private void Awake()
        {
            dropdown = GetComponent<TMP_Dropdown>();
        }

        private void Start()
        {
            dropdown.onValueChanged.AddListener(OnLanguageChanged);
            PopulateDropdown();
        }

        private void OnLanguageChanged(int value)
        {
            dropdown.captionText.text = LocalizationManager.GetAvailableLanguages()[dropdown.value];
            dropdownCaption.SetText(dropdown.captionText.text);
            LocalizationManager.Instance.LanguageChanged((Language)value);
        }

        private void PopulateDropdown()
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(LocalizationManager.GetAvailableLanguages());
            dropdown.value = (int)LocalizationManager.currentLanguage;
            OnLanguageChanged((int)LocalizationManager.currentLanguage);
        }
    }
}