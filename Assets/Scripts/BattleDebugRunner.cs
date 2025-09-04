using Data;
using Domain.Combat;
using UnityEngine;
using Domain.Core;

public class BattleDebugRunner : MonoBehaviour
{
    [Header("Оружие")]
    [SerializeField] private WeaponConfig heroWeapon;
    [SerializeField] private WeaponConfig monsterWeapon;

    private void Start()
    {
        var heroStats = new Stats(2, 3, 2 );
        var monsterStats = new Stats(1, 2, 1 );
        
        var hero = new Hero("Герой", heroStats, maxHp: 10, weapon: heroWeapon);
        var monster = new Monster("Гоблин", monsterStats, maxHp: 8, weapon: monsterWeapon, reward: monsterWeapon);
        
        var resolver = new CombatResolver();
        
        var result = resolver.Simulate(hero, monster);
        
        Debug.Log(result.Log);
    }
}