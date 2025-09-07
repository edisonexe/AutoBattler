using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class WeaponRepository : MonoBehaviour
    {
        [SerializeField] private WeaponConfig[] _allWeapons;
        
        private Dictionary<string, WeaponConfig> _weaponsByName;

        public IReadOnlyList<WeaponConfig> AllWeapons => _allWeapons;

        private void Awake()
        {
            IndexByNames();
        }

        private void IndexByNames()
        {
            _weaponsByName = new Dictionary<string, WeaponConfig>();

            foreach (var weapon in _allWeapons)
            {
                if (weapon == null) continue;
                if (!_weaponsByName.ContainsKey(weapon.Name))
                    _weaponsByName[weapon.Name] = weapon;
            }
        }
        
        public WeaponConfig GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            return _weaponsByName.TryGetValue(name, out var w) ? w : null;
        }
    }
}