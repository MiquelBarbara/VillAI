using System;
using System.Collections;
using System.Collections.Generic;
using EntitiesRelated.Core.Data;
using Systems.Inputs;
using UI.Presentation.PanelText;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Panels and Menus")]
        [SerializeField] private AcceptRejectPanel acceptRejectPanel;
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private MainMenu mainMenu;

        [Header("Display Components")]
        [SerializeField] private PanelPresenter panelPresenter;
        [SerializeField] private BubbleFactory bubbleFactory;
        [SerializeField] private InputHandler inputHandler;
        
        
        [Header("Trade")]
        [SerializeField] private InventoryPanel character1InventoryPanel;
        [SerializeField] private InventoryPanel character2InventoryPanel;
        
        [SerializeField] private InventoryController inventoryController;
        
        public void ShowAcceptRejectPanel(string message, System.Action onAccept = null, System.Action onCancel = null)
        {
            acceptRejectPanel.gameObject.SetActive(true);
            acceptRejectPanel.Show(message, onAccept, onCancel);
        }
        public void HideAcceptRejectPanel()
        {
            acceptRejectPanel.gameObject.SetActive(false);
        }
        
        public void ShowTwoInventoryPanels(ItemContainer character1Items, ItemContainer character2Items)
        {
            character1InventoryPanel.Inject(character1Items);
            character2InventoryPanel.Inject(character2Items);
            
            character1InventoryPanel.gameObject.SetActive(true);
            character2InventoryPanel.gameObject.SetActive(true);
        }
        
        public void HideTwoInventoryPanels()
        {
            character1InventoryPanel.Hide();
            character2InventoryPanel.Hide();
        }
        
        public void ShowBubble(Transform target, List<string> lines, float duration, Action onComplete = null)
        {
            bubbleFactory.ShowBubble(target, lines, duration, () => {
                onComplete?.Invoke();
            });
        }
        
        public IEnumerator ShowPanel(List<string> lines, CharacterData characterData = null, Action onComplete = null)
        {
            bool isComplete = false;
            Action wrappedCallback = () =>
            {
                HidePanel();
                onComplete?.Invoke();
                isComplete = true;
            };

            panelPresenter.Initialize(lines, characterData, wrappedCallback);
            panelPresenter.ShowPanel();
            yield return new WaitUntil(() => isComplete);
        }
        
        public IEnumerator ShowBubble(Transform target, List<string> lines, float durationPerLine = 3f)
        {
            bool isComplete = false;
        
            ShowBubble(target, lines, durationPerLine, () => {
                isComplete = true;
            });
        
            yield return new WaitUntil(() => isComplete);
        }
        
        public void ShowToolbar()
        {
            inventoryController.OpenToolbar();
        }

        public void HideToolbar()
        {
            inventoryController.CloseToolbar();
        }
        
        public void HidePanel()
        {
            panelPresenter.HidePanel();
        }
        
        public IEnumerator GetPlayerInput(Action<string> onInputReceived)
        {
            yield return StartCoroutine(inputHandler.GetPlayerInput(onInputReceived));
        }

        public void TogglePauseMenu()
        {
            if (pauseMenu.gameObject.activeSelf)
                pauseMenu.Resume();
            else
                pauseMenu.gameObject.SetActive(true);
        }

        public void StartNewGame()
        {
            mainMenu.StartNewGame();
        }

        public void ExitGame()
        {
            mainMenu.ExitGame();
        }
        
        
    }
}
