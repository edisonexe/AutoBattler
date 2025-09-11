using Domain.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Data
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private int _baseDamage;
        [SerializeField] private DamageType _type;
        [SerializeField] private Sprite  _sprite;

        public Sprite Sprite => _sprite;
        public string Name => _name;
        public int BaseDamage => _baseDamage;
        public DamageType Type => _type;
    }
}