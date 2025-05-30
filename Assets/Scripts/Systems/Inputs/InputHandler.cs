using System;
using System.Collections;
using Game.Utilities.DependencyInjection;
using TMPro;
using UnityEngine;

namespace Systems.Inputs
{

    /// <summary>
    ///     The InputHandler class manages player input by interacting with an input buffer and input panel.
    ///     It is responsible for displaying the input panel, waiting for input, and invoking a callback with the player's
    ///     input.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        private string _input;
        private bool _isSubmitted;
        
        private void OnEnable()
        {
            inputField.onEndEdit.AddListener(OnUserInputSubmitted);
        }

        private void OnDisable()
        {
            inputField.onEndEdit.RemoveListener(OnUserInputSubmitted);
        }

        private void OnUserInputSubmitted(string inputText)
        {
            if (string.IsNullOrEmpty(inputText)) return;
            _input = inputText;
            _isSubmitted = true;
        }
        

        /// <summary>
        ///     Displays the input panel and waits for the player's input, then invokes the provided callback with the input.
        /// </summary>
        /// <param name="onInputReceived">Callback function that will be called once the input is received.</param>
        public IEnumerator GetPlayerInput(Action<string> onInputReceived)
        {
            ShowInputPanel();
            inputField.ActivateInputField();
            _isSubmitted = false;
    
            yield return new WaitUntil(() => _isSubmitted);
            
            HideInputPanel();
            onInputReceived?.Invoke(_input);
        }
        
        /// <summary>
        ///     Displays the input panel and makes the input field active for user interaction.
        /// </summary>
        public void ShowInputPanel()
        {
            gameObject.SetActive(true);
            inputField.gameObject.SetActive(true);
        }

        /// <summary>
        ///     Hides the input panel, clears the input field, and deactivates the input field.
        /// </summary>
        public void HideInputPanel()
        {
            gameObject.SetActive(false);
            inputField.text = string.Empty;
            inputField.gameObject.SetActive(false);
        }
    }
}