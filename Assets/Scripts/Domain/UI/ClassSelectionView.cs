using System;
using Domain.Core;
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
        
        private Hero _hero;
        private ClassSelection _classSelection;

        private void Awake()
        {
            _classSelection = new ClassSelection();
        }

        private void OnEnable()
        {
            _rogueCls.onClick.AddListener(() => OnClassPicked(HeroClass.Rogue));
            _warriorCls.onClick.AddListener(() => OnClassPicked(HeroClass.Warrior));
            _barbarianCls.onClick.AddListener(() => OnClassPicked(HeroClass.Barbarian));
        }

        private void OnDisable()
        {
            _rogueCls.onClick.RemoveListener(() => OnClassPicked(HeroClass.Rogue));
            _warriorCls.onClick.RemoveListener(() => OnClassPicked(HeroClass.Warrior));
            _barbarianCls.onClick.RemoveListener(() => OnClassPicked(HeroClass.Barbarian));
        }

        public void ShowPanel()
        {
            _panel.SetActive(true);
        }
        
        public void HidePanel()
        {
            if (_panel) _panel.SetActive(false);
        }

        private void OnClassPicked(HeroClass cls)
        {
            _classSelection.ApplyPick(_hero, cls);
            HidePanel();
        }
        
    }
}