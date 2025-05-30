using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class TypeOutText: MonoBehaviour
    {
        [SerializeField] TMP_Text tmpText;
        
        [SerializeField] private float timePerLetter = 0.05f; // Time it takes to display one letter.

        private int _letterCount; // The number of letters that should be visible.
        private string _lineToShow; // The current line of text being displayed.
        private float _totalTimeToType; // Total time to type out the line and the current elapsed time.
        private int _currentTextLine; // Index of the current line being displayed.
        private float _currentTime; // Total time to type out the line and the current elapsed time.

        public float visibleTextPercent; // Percentage of the current line that is visible.
        private List<string> Dialogue { get; set; }
        
        public event Action OnComplete;
        public string GetLineToShow => _lineToShow;
        public bool IsLineComplete => visibleTextPercent >= 1f;

        public void Initialize(List<string> lines)
        {
            Dialogue = lines;
            _currentTextLine = 0;
            CycleLine();
        }

        public void Update()
        {
            if (Dialogue == null) return;
            TypeOut();
            WriteText(_lineToShow, _letterCount);
        }
        
        public void WriteText(string lineToShow, int letterCount)
        {
            tmpText.text = lineToShow[..letterCount];
        }
        
        public void UpdateText()
        {
            _letterCount = (int)(_lineToShow.Length * visibleTextPercent);
        }
        
        // Simulates typing by gradually revealing the text over time.
        public void TypeOut()
        {
            if (visibleTextPercent >= 1f) return;

            _currentTime += Time.deltaTime;
            visibleTextPercent = _currentTime / _totalTimeToType;
            visibleTextPercent = Mathf.Clamp(visibleTextPercent, 0, 1f);
            UpdateText();
        }

        // Moves to the next line of dialogue and resets typing parameters.
        public void CycleLine()
        {
            _lineToShow = Dialogue[_currentTextLine];
            _totalTimeToType = _lineToShow.Length * timePerLetter;
            _currentTime = 0f;
            visibleTextPercent = 0f;

            _currentTextLine += 1;
        }
        
        public void PushText()
        {
            if (visibleTextPercent < 1f)
            {
                visibleTextPercent = 1f;
                UpdateText();
                return;
            }

            if (_currentTextLine >= Dialogue.Count)
                Complete();
            else
                CycleLine();
        }
        
        
        
        public void Complete()
        {
            OnComplete?.Invoke();
            Empty();
        }
        
        public void Empty()
        {
            tmpText.text = "";
            Dialogue = null;
        }
    }
}