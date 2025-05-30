using System;
using System.Collections.Generic;
using EntitiesRelated.Core.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Presentation.PanelText
{
    /// <summary>
    /// Manages dialogue panel UI, including text, name, and character icon presentation.
    /// </summary>
    public class PanelPresenter : MonoBehaviour
    {
        [SerializeField] public TypeOutText targetText;
        [SerializeField] private TypeOutText targetTextNoIcon;

        [SerializeField] private TMP_Text nameText;
        [SerializeField] private GameObject iconPanel;
        [SerializeField] private Image icon;
        
        private Action _onCompleteCallback;

        private void Start()
        {
            HidePanel();
        }
        
        private void OnEnable()
        {
            targetText.OnComplete += HandleTextComplete;
            targetTextNoIcon.OnComplete += HandleTextComplete;
        }
    
        private void OnDisable()
        {
            targetText.OnComplete -= HandleTextComplete;
            targetTextNoIcon.OnComplete -= HandleTextComplete;
        }
        
        private void HandleTextComplete()
        {
            _onCompleteCallback?.Invoke();
        }

        public void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            
            if(targetText.isActiveAndEnabled)
            {
                targetText.PushText();
            }
            else if (targetTextNoIcon.isActiveAndEnabled)
            {
                targetTextNoIcon.PushText();
            }
        }

        public void Initialize(List<string> lines, CharacterData characterData = null, Action onComplete = null)
        {
            _onCompleteCallback = onComplete;
        
            if (characterData == null)
            {
                targetTextNoIcon.Initialize(lines);
            }
            else
            {
                icon.sprite = characterData.icon;
                nameText.text = characterData.GetCharacterName();
                targetText.Initialize(lines);
            }
        }
        
        /// <summary>
        /// Shows the panel and adjusts layout based on icon presence.
        /// </summary>
        public void ShowPanel()
        {
            gameObject.SetActive(true);

            bool hasIcon = icon.sprite != null;
            
            iconPanel.SetActive(hasIcon);
            icon.gameObject.SetActive(hasIcon);

            nameText.gameObject.SetActive(hasIcon);

            targetText.gameObject.SetActive(hasIcon);

            targetTextNoIcon.gameObject.SetActive(!hasIcon);
        }

        /// <summary>
        /// Hides all elements of the panel.
        /// </summary>
        public void HidePanel()
        {
            gameObject.SetActive(false);
            iconPanel.SetActive(false);
            icon.gameObject.SetActive(false);

            targetText.gameObject.SetActive(false);

            targetTextNoIcon.gameObject.SetActive(false);
            nameText.gameObject.SetActive(false);
        }
    }
}