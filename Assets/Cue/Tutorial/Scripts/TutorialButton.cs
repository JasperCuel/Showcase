using Cue.Core;
using Cue.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SugarVita.UI.Tutorial
{
    [RequireComponent(typeof(Button))]
    public class TutorialButton : MonoBehaviour
    {
        public TutorialTimeline tutorialTimeline;

        private TutorialPanelUI tutorial;
        private Button button;

        private void Awake() => button = GetComponent<Button>();

        private void Start()
        {
            button.onClick.AddListener(OnTutorialButton);
            if (tutorialTimeline == null || !tutorialTimeline.ContainsTutorials)
            {
                DebugLogger.LogWarning($"{name} has been disabled because there was no valid tutorial timeline referenced.", DebugType.UI);
                gameObject.SetActive(false);
            }

            CueUtility.InvokeNextFrame(this, () => StartCoroutine(CheckFirstTimePlaying()));
        }

        private IEnumerator CheckFirstTimePlaying()
        {
            yield return new WaitUntil(() => button.interactable);

            if (PlayerPrefs.GetInt(tutorialTimeline.tutorialName, 0) == 0)
                UIManager.Instance.OnEmptyPopupQueue = () => OnTutorialButton();
        }

        public void OnTutorialButton()
        {
            UIManager.Instance.ShowPanel(PanelType.TutorialPanel, true, false);
            tutorial = FindObjectOfType<TutorialPanelUI>();
            tutorial.StartTutorial(tutorialTimeline);
        }
    }
}