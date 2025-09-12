using Domain.Rules;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "MonsterClassConfig", menuName = "Configs/MonsterClassConfig")]
    public class MonsterClassConfig : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private MonsterClass _monsterClass;
        [SerializeField] private int _maxHp;
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private int _strength;
        [SerializeField] private int _agility;
        [SerializeField] private int _stamina;
        [SerializeField] private WeaponConfig _reward;

        public string Name => _name;
        public MonsterClass MonsterClass => _monsterClass;
        public int MaxHp => _maxHp;
        public WeaponConfig WeaponConfig => _weaponConfig;
        public int Strength => _strength;
        public int Agility => _agility;
        public int Stamina => _stamina;
        public WeaponConfig Reward => _reward;
    }
}