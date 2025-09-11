using System;
using Domain.Rules;
using UnityEngine;
using UnityEngine.UI;

namespace Domain.UI
{
    public class ClassSelectionView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _rogueCls;
        [SerializeField] private Button _warriorCls;
        [SerializeField] private Button _barbarianCls;
        
        private ClassSelection _classSelection;
        
        public event Action<HeroClass> OnClassPicked;

        private void Awake() => _classSelection = new ClassSelection();
        
        private void OnEnable()
        {
            _rogueCls.onClick.AddListener(OnRogue);
            _warriorCls.onClick.AddListener(OnWarrior);
            _barbarianCls.onClick.AddListener(OnBarbarian);
        }
        private void OnDisable()
        {
            _rogueCls.onClick.RemoveListener(OnRogue);
            _warriorCls.onClick.RemoveListener(OnWarrior);
            _barbarianCls.onClick.RemoveListener(OnBarbarian);
        }
        private void OnRogue() => Pick(HeroClass.Rogue);
        private void OnWarrior() => Pick(HeroClass.Warrior);
        private void OnBarbarian() => Pick(HeroClass.Barbarian);
        
        public void ShowPanel() => _panel.SetActive(true);
        public void HidePanel() => _panel.SetActive(false);
        
        private void Pick(HeroClass cls)
        {
            OnClassPicked?.Invoke(cls);
        }
        
    }
}