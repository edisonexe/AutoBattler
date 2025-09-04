using Domain.Core;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private int _baseDamage;
        [SerializeField] private DamageType _type;
        
        public string Name => _name;
        public int BaseDamage => _baseDamage;
        public DamageType Type => _type;
    }
}