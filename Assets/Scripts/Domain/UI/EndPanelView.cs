using System;
using Domain.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Domain.UI
{
    public class EndPanelView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;

        private const string WON = "Hero won!";
        private const string LOST = "Hero lost!";
        
        public event Action OnPlayAgain;
        
        private void Awake()
        {
            _panel.SetActive(false);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(PlayAgainClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(PlayAgainClicked);
        }
        
        private void PlayAgainClicked()
        {
            // Debug.Log($"[EndPanelView] PlayAgain clicked on {name} (id={GetInstanceID()})");
            
            OnPlayAgain?.Invoke();
            HidePanel();
        }
        
        public void ShowPanel(BattleOutcome outcome)
        {
            _text.text = outcome == BattleOutcome.HeroWon ? WON : LOST; 
            _panel.SetActive(true);
        }

        public void HidePanel()
        {
            _panel.SetActive(false);
        }
    }
}