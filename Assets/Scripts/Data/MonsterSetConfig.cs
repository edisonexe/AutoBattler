using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "MonsterSetConfig", menuName = "Configs/MonsterSetConfig", order = 0)]
    public class MonsterSetConfig : ScriptableObject
    {
        [SerializeField] private MonsterClassConfig[] _monsters;
        public MonsterClassConfig[] Monsters => _monsters;
    }
}