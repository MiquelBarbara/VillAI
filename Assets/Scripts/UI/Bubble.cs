using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField] private RectTransform _bubbleRectTransform;
        [SerializeField] private Image _bubbleBackground;
        [SerializeField] private TypeOutText _typeOutText;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _padding = 20f;
        [Header("Text Constraints")]
        [SerializeField] private float _maxTextWidth = 300f;
        
        private Transform _followTarget;
        private Vector3 _offset;
        
        private CountdownTimer _timer;
        public event Action OnComplete;
        
        private void OnEnable()
        {
            _typeOutText.OnComplete += OnCompleteBubble;
        }
    
        private void OnDisable()
        {
            _typeOutText.OnComplete -= OnCompleteBubble;
        }
        
        private void OnCompleteBubble()
        {
            OnComplete?.Invoke();
            Destroy(gameObject);
        }

        public void Initialize(List<string> dialogue, Transform followTarget, float durationPerLine, Vector3 offset)
        {
            _followTarget = followTarget;
            _offset = offset;
            _typeOutText.Initialize(dialogue);
            
            _timer = new CountdownTimer(durationPerLine);
            _timer.OnTimerStop += () =>
            {
                _typeOutText.PushText();
            };
            _timer.Start();
            
            UpdateBubbleSize();
        }

        private void Update()
        {
            if (_followTarget == null) return;
            
            UpdateBubbleSize();
            _bubbleRectTransform.position = _followTarget.position + _offset;

            if (!_typeOutText.IsLineComplete) return;
            _timer.Tick(Time.deltaTime);
            if (!_timer.IsFinished) return;
            
            _timer.Reset();
            _timer.Start();
        }

        private void UpdateBubbleSize()
        {
            TMP_Text tmpText = _typeOutText.GetComponent<TMP_Text>();
            
            // 1. Forzar ancho máximo y actualizar layout
            tmpText.rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal, 
                _maxTextWidth
            );
            
            // 2. Calcular tamaño del texto con límite de ancho
            Vector2 textSize = tmpText.GetPreferredValues(
                tmpText.text, 
                _maxTextWidth, 
                0f // Altura ilimitada
            );

            // 3. Actualizar tamaño del fondo
            _bubbleBackground.rectTransform.sizeDelta = new Vector2(
                _maxTextWidth + _padding, 
                textSize.y + _padding
            );

            // 4. Forzar actualización inmediata
            LayoutRebuilder.ForceRebuildLayoutImmediate(_bubbleBackground.rectTransform);
        }

        private void UpdatePosition()
        {
            
        }
    }
}