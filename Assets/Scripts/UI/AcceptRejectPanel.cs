using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Displays a confirmation panel with Accept and Cancel buttons, invoking corresponding actions.
    /// </summary>
    public class AcceptRejectPanel: MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button cancelButton;

        private Action onAccept;
        private Action onCancel;

        private void Awake()
        {
            acceptButton.onClick.AddListener(Accept);
            cancelButton.onClick.AddListener(Cancel);
            
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Shows the panel with a message and callback actions.
        /// </summary>
        /// <param name="message">Message to display.</param>
        /// <param name="acceptAction">Action on accept.</param>
        /// <param name="cancelAction">Action on cancel (optional).</param>
        public void Show(string message, Action acceptAction, Action cancelAction = null)
        {
            messageText.text = message;
            onAccept = acceptAction;
            onCancel = cancelAction;

            gameObject.SetActive(true);
        }

        private void Accept()
        {
            onAccept?.Invoke();
            Close();
        }

        private void Cancel()
        {
            onCancel?.Invoke();
            Close();
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }
        
    }
}