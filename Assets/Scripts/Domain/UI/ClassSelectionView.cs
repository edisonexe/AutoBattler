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
            _rogueCls.onClick.AddListener(() => Pick(HeroClass.Rogue));
            _warriorCls.onClick.AddListener(() => Pick(HeroClass.Warrior));
            _barbarianCls.onClick.AddListener(() => Pick(HeroClass.Barbarian));
        }

        private void OnDisable()
        {
            _rogueCls.onClick.RemoveListener(() => Pick(HeroClass.Rogue));
            _warriorCls.onClick.RemoveListener(() => Pick(HeroClass.Warrior));
            _barbarianCls.onClick.RemoveListener(() => Pick(HeroClass.Barbarian));
        }
        
        public void ShowPanel() => _panel.SetActive(true);
        public void HidePanel() => _panel.SetActive(false);
        
        private void Pick(HeroClass cls)
        {
            OnClassPicked?.Invoke(cls);
        }
        
    }
}