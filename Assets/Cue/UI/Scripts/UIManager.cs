using Cue.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cue.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [Header("Panels")]
        [SerializeField]
        private Transform panelParent;

        [SerializeField]
        private bool showPanelAsDefault = true;

        [SerializeField]
        [ShowIf(nameof(showPanelAsDefault), true, null)]
        private UIPanel sceneDefaultPanel;

        [SerializeField]
        [Tooltip("The maximum amount of previous panels to go back to")]
        private int maxPlaybackPanels = 5;

        [SerializeField]
        private List<UIPanel> panelPrefabs;

        private List<UIPanel> activePanels;
        private UIPanel currentActivePanel;

        private List<PanelType> previousPanels;

        ////////////////////////////////////////////////////////////////////

        [Header("Popups")]
        [SerializeField]
        private Transform popupParent;

        [SerializeField]
        private Color raycastBlockColor = Color.white;

        [Range(0.01f, 0.7f)]
        [SerializeField]
        private float animationSpeed = 0.1f;

        [SerializeField]
        private List<Popup> genericPopupPrefabs;

        [SerializeField]
        private List<UniquePopup> uniquePopupPrefabs;

        private Queue<PopupInfo> popupQueue;
        private List<Popup> currentActivePopups;
        private RectTransform raycastBlocker;

        private List<Action> onEmptyPopupQueue = new();

        public Action OnEmptyPopupQueue
        {
            set
            {
                if (popupQueue != null && currentActivePopups.Count > 0)
                    onEmptyPopupQueue.Add(value);
                else value?.Invoke();
            }
        }

        public bool IsPopupActive
        { get { return currentActivePopups.Count > 0; } }

        ////////////////////////////////////////////////////////////////////

        [Header("Scene References")]
        [SerializeField]
        private SceneReference previousScene;

        [SerializeField]
        private SceneReference nextScene;

        ////////////////////////////////////////////////////////////////////

        #region Unity Messages

        private void Awake()
        {
            activePanels = new List<UIPanel>();
            currentActivePanel = null;
            currentActivePopups = new List<Popup>();
            popupQueue = new Queue<PopupInfo>();
            previousPanels = new List<PanelType>();
        }

        private void Start()
        {
            if (showPanelAsDefault && sceneDefaultPanel == null && panelPrefabs != null && panelPrefabs.Count > 0)
                sceneDefaultPanel = panelPrefabs[0];

            CheckForPanelsInScene();
            ValidatePanels();
            ValidatePopups();

            if (sceneDefaultPanel != null)
                ShowPanel(sceneDefaultPanel.type);
        }

        /// <summary>
        /// Check for active panels in scene
        /// </summary>
        private void CheckForPanelsInScene()
        {
            UIPanel[] scenePanels = FindObjectsOfType<UIPanel>();
            foreach (UIPanel panel in scenePanels)
            {
                UIPanel panelToSwap = panelPrefabs.SingleOrDefault(x => x.type.Equals(panel.type));
                if (panelToSwap)
                {
                    panelPrefabs.Remove(panelToSwap);
                    panelPrefabs.Add(panel);
                    panel.transform.SetParent(panelParent, false);
                    activePanels.Add(panel);
                    panel.gameObject.SetActive(panel.type.Equals(sceneDefaultPanel.type));
                    panel.gameObject.name += "(Scene)";
                }
                else
                    DebugLogger.LogWarning($"{panel.name} is not being referenced by {Instance.name} ({nameof(UIManager)}) and might not function correctly", DebugType.UI);
            }
        }

        /// <summary>
        /// Check for unique panels
        /// </summary>
        private void ValidatePanels()
        {
            List<PanelType> uniqueIds = new();
            foreach (UIPanel panel in panelPrefabs)
                uniqueIds.Add(panel.type);
            if (uniqueIds.Count != uniqueIds.Distinct().Count())
                DebugLogger.LogWarning($"Duplicate panel types exist in {Instance.name}", DebugType.UI);
        }

        /// <summary>
        /// Check if popups are valid
        /// </summary>
        private void ValidatePopups()
        {
            foreach (Popup popupPrefab in genericPopupPrefabs)
                if (popupPrefab.type.Equals(PopupType.Unique))
                    DebugLogger.LogWarning($"{popupPrefab.name} is of popup type Unique and should instead be referenced in 'Unique Popup Prefabs'", DebugType.UI);
            foreach (Popup popupPrefab in uniquePopupPrefabs)
                if (!popupPrefab.type.Equals(PopupType.Unique))
                    DebugLogger.LogWarning($"{popupPrefab.name} is not of popup type Unique and should instead be referenced in 'Generic Popup Prefabs'", DebugType.UI);

            List<PopupType> uniqueIds = new();
            foreach (Popup popup in genericPopupPrefabs)
                uniqueIds.Add(popup.type);
            if (uniqueIds.Count != uniqueIds.Distinct().Count())
                DebugLogger.LogWarning($"Duplicate popup types exist in {Instance.name}", DebugType.UI);
        }

        #endregion Unity Messages

        #region Panels

        /// <summary>
        /// Show new panel and hide the current active panel
        /// </summary>
        /// <param name="panelType">The panelType is set in the <see cref="UIPanel"/></param>
        public void ShowPanel(PanelType panelType, bool addToPreviousPanels = true, bool hidePreviousPanel = true)
        {
            if (currentActivePanel != null && currentActivePanel.type.Equals(panelType))
            {
                DebugLogger.LogWarning($"{panelType} is already active.", DebugType.UI);
                return;
            }
            UIPanel panelToActivate = panelPrefabs.FirstOrDefault(x => x.type.Equals(panelType));
            if (panelToActivate == null)
            {
                DebugLogger.LogWarning($"Cannot show {panelType} because it is not listed in {Instance.name}", DebugType.UI);
                return;
            }

            if (currentActivePanel != null) //Disable previous panel
            {
                if (addToPreviousPanels)
                {
                    if (previousPanels.Count > maxPlaybackPanels)
                        previousPanels.RemoveAt(0);
                    previousPanels.Add(currentActivePanel.type);
                }
                if (hidePreviousPanel)
                    currentActivePanel.gameObject.SetActive(false);
            }
            UIPanel instantiatedPanel = activePanels.FirstOrDefault(x => x.type.Equals(panelToActivate.type));
            if (instantiatedPanel != null) //Panel is already instantiated
                instantiatedPanel.gameObject.SetActive(true);
            else //Panel isn't instantiated yet
            {
                instantiatedPanel = Instantiate(panelToActivate, panelParent);
                activePanels.Add(instantiatedPanel);
            }
            instantiatedPanel.transform.SetAsLastSibling();
            RectTransform rt = instantiatedPanel.GetComponent<RectTransform>();
            currentActivePanel = instantiatedPanel;
        }

        /// <summary>
        /// Shows previously shown panel (up to <see cref="maxPlaybackPanels"/> back)
        /// </summary>
        public void ShowPreviousPanel()
        {
            if (previousPanels.Count > 0)
            {
                ShowPanel(previousPanels[^1], false);
                previousPanels.RemoveAt(previousPanels.Count - 1);
            }
            else
            {
                currentActivePanel.gameObject.SetActive(false);
                currentActivePanel = null;
            }
        }

        #endregion Panels

        #region Popups

        /// <summary>
        /// Enqueue a popup, it wil be shown after the last one has been closed
        /// </summary>
        /// <param name="info">Information about the popup in a class derived from <see cref="PopupInfo"/></param>
        /// <param name="force">Force this popup to be shown on top</param>
        public void QueuePopup(PopupInfo info, bool force = false)
        {
            if (force)
            {
                popupQueue.Enqueue(info);
                ShowNextPopupInLine();
            }
            else
            {
                popupQueue.Enqueue(info);
                if (currentActivePopups != null && currentActivePopups.Count <= 0 && popupQueue != null && popupQueue.Count > 0)
                    ShowNextPopupInLine();
            }
        }

        /// <summary>
        /// Hides the currently displayed popup
        /// </summary>
        public void HideCurrentPopup()
        {
            Popup currentPopup = currentActivePopups[^1];
            if (currentPopup == null)
                return;
            SetRaycastBlocker(false);
            LeanTween.scale(currentPopup.gameObject, Vector3.zero, animationSpeed).setOnComplete(() => OnHidePopup(currentPopup));
        }

        private void OnHidePopup(Popup currentPopup)
        {
            currentActivePopups.Remove(currentPopup);
            Destroy(currentPopup.gameObject);
            if (popupQueue != null && popupQueue.Count > 0 && currentActivePopups.Count <= 0)
                ShowNextPopupInLine();
            else
            {
                for (int i = 0; i < onEmptyPopupQueue?.Count; i++)
                {
                    onEmptyPopupQueue[i]();
                }

                onEmptyPopupQueue.Clear();
            }
        }

        private void ShowNextPopupInLine()
        {
            PopupInfo info = popupQueue.Dequeue();
            if (info != null)
                ShowPopup(info);
        }

        private void ShowPopup(PopupInfo info)
        {
            Popup instantiatedPopup = null;
            switch (info.type)
            {
                case PopupType.Information:
                    InformationPopup informationPopup = Instantiate(genericPopupPrefabs.FirstOrDefault(x => x.type.Equals(info.type)), popupParent).GetComponent<InformationPopup>();
                    instantiatedPopup = informationPopup;
                    InformationPopupInfo informationPopupInfo = (InformationPopupInfo)info;
                    informationPopup.Setup(informationPopupInfo);
                    currentActivePopups.Add(informationPopup);
                    break;

                case PopupType.Confirmation:
                    ConfirmationPopup confirmationPopup = Instantiate(genericPopupPrefabs.FirstOrDefault(x => x.type.Equals(info.type)), popupParent).GetComponent<ConfirmationPopup>();
                    instantiatedPopup = confirmationPopup;
                    ConfirmationPopupInfo confirmationPopupInfo = (ConfirmationPopupInfo)info;
                    confirmationPopup.Setup(confirmationPopupInfo);
                    currentActivePopups.Add(confirmationPopup);
                    break;

                case PopupType.Unique:
                    UniquePopupInfo popupInfo = info as UniquePopupInfo;
                    UniquePopup popupToInstantiate = uniquePopupPrefabs.FirstOrDefault(x => x.uniquePopupId.Equals(popupInfo.uniquePopupId));
                    if (popupToInstantiate == null)
                    {
                        DebugLogger.LogWarning($"Unique popup with id: {popupInfo.uniquePopupId} is not listed in {Instance.name} ({nameof(UIManager)})", DebugType.UI);
                        return;
                    }
                    UniquePopup popup = Instantiate(popupToInstantiate, popupParent);
                    instantiatedPopup = popup;
                    if (popup.closeButton != null)
                        popup.closeButton.onClick.AddListener(() => HideCurrentPopup());
                    currentActivePopups.Add(popup);
                    break;

                case PopupType.Banner:
                    BannerPopup bannerPopup = Instantiate(genericPopupPrefabs.FirstOrDefault(x => x.type.Equals(info.type)), popupParent).GetComponent<BannerPopup>();
                    instantiatedPopup = bannerPopup;
                    BannerPopupInfo bannerPopupInfo = (BannerPopupInfo)info;
                    bannerPopup.Setup(bannerPopupInfo);
                    currentActivePopups.Add(bannerPopup);
                    break;

                default:
                    DebugLogger.LogError($"Popuptype {info.type} has not been setup properly", DebugType.UI);
                    break;
            }
            if (instantiatedPopup != null)
            {
                instantiatedPopup.gameObject.transform.localScale = Vector3.zero;
                LeanTween.scale(instantiatedPopup.gameObject, Vector3.one, animationSpeed);
            }
            SetRaycastBlocker(true);
        }

        private void SetRaycastBlocker(bool isOn)
        {
            if (raycastBlocker == null)
                CreateRaycastBlocker();
            raycastBlocker.gameObject.SetActive(isOn);
            if (raycastBlocker.GetSiblingIndex() != 0)
                raycastBlocker.SetSiblingIndex(0);
            popupParent.SetSiblingIndex(popupParent.parent.childCount - 1);
        }

        private void CreateRaycastBlocker()
        {
            GameObject go = new("RaycastBlocker");
            go.transform.SetParent(popupParent);
            raycastBlocker = go.AddComponent<RectTransform>();
            raycastBlocker.transform.localPosition = Vector3.zero;
            raycastBlocker.pivot = Vector2.one / 2;
            raycastBlocker.localScale = Vector3.one;
            raycastBlocker.anchorMin = Vector2.zero;
            raycastBlocker.anchorMax = Vector2.one;

            Image image = raycastBlocker.gameObject.AddComponent<Image>();
            image.raycastTarget = true;
            image.sprite = null;
            image.color = raycastBlockColor;
        }

        #endregion Popups

        #region Scenes

        /// <summary>
        /// Load a specific scene by scene reference
        /// </summary>
        public void LoadScene(SceneReference scene, bool showLoadingScreen = false)
        {
            if (showLoadingScreen)
                ShowPanel(PanelType.LoadingPanel, false);
            CustomSceneManager.LoadScene(scene);
        }

        /// <summary>
        /// Load the previous scene
        /// </summary>
        public void LoadPreviousScene(bool showLoadingScreen = false)
        {
            LoadScene(previousScene, showLoadingScreen);
        }

        /// <summary>
        /// Load the next scene
        /// </summary>
        public void LoadNextScene(bool showLoadingScreen = false)
        {
            LoadScene(nextScene, showLoadingScreen);
        }

        #endregion Scenes
    }
}