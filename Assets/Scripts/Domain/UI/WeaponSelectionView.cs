using System;
using Domain.Core;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Domain.UI
{
    public class WeaponSelectionView  : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _takeButton;
        [SerializeField] private Button _skipButton;
        [SerializeField] private TMP_Text _currentWeaponText;
        [SerializeField] private TMP_Text _rewardWeaponText;
        [SerializeField] private Image _currentSprite;
        [SerializeField] private Image _rewardSprite;

        private Weapon _current;
        private Weapon _reward;

        public event Action<Weapon> OnTake;
        public event Action OnSkip;

        public void Show(Weapon current, Weapon reward)
        {
            _current = current;
            _reward = reward;

            _currentSprite.sprite = _current.Sprite;
            _rewardSprite.sprite = _reward.Sprite;
            
            _currentWeaponText.text = current != null ? $"Current: {current.Name}" : "";
            _rewardWeaponText.text  = reward  != null ? $"Reward: {reward.Name}"  : "";

            // _takeButton.onClick.RemoveAllListeners();
            // _skipButton.onClick.RemoveAllListeners();

            _takeButton.onClick.AddListener(() => { Hide(); OnTake?.Invoke(_reward); });
            _skipButton.onClick.AddListener(() => { Hide(); OnSkip?.Invoke(); });

            _panel.SetActive(true);
        }

        public void Hide()
        {
            _panel.SetActive(false);
            _takeButton.onClick.RemoveAllListeners();
            _skipButton.onClick.RemoveAllListeners();
        }
    }
}