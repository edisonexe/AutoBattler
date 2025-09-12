using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Domain.Combat.Effects;
using Domain.Core;
using Domain.UI.Interfaces;
using UnityEngine;

namespace Domain.Combat
{
    public class BattleManager
    {
        public async UniTask<BattleResult> FightAsync(Hero hero, Fighter monster, float stepDelaySeconds = 2f,
            int roundCap = 200, IUIEvents uiEvents = null)
        {
            hero.ResetForCombat();
            monster.ResetForCombat();
            
            uiEvents?.OnBind(hero, monster);
            
            Fighter attacker = hero.Stats.Agility >= monster.Stats.Agility ? hero : monster;
            Fighter defender = ReferenceEquals(attacker, hero) ? monster : hero;
            await UniTask.Delay(TimeSpan.FromSeconds(stepDelaySeconds));
            int round = 0;
            while (hero.IsAlive && monster.IsAlive && round < roundCap)
            {
                round++;
                attacker.IncrementTurn();
                Debug.Log($"[{attacker.Name}: turn {attacker.TurnsTaken}]");
                
                var ctx = new EffectContext(attacker, defender);
                
                // 1) шанс попадания: rnd [1 .. atk.Agi + def.Agi]
                int atkAgi = attacker.Stats.Agility;
                int defAgi = defender.Stats.Agility;
                int roll = Utils.Utils.RandomInt(1, Math.Max(2, atkAgi + defAgi));
                bool miss = roll <= defAgi;

                if (miss)
                {
                    uiEvents?.OnMiss(defender);
                    Debug.Log("MISS");
                    await UniTask.Delay(TimeSpan.FromSeconds(stepDelaySeconds));
                }
                else
                {
                    // 2) базовый урон: оружие атакующего + его сила
                    int damage = attacker.GetBaseDamage();
                    
                    // 3) эффекты атаки
                    foreach (var e in attacker.Attack.OrderBy(e => e.Priority))
                    {
                        int before = damage;
                        damage = Math.Max(0, e.ModifyOutgoingDamage(ctx, damage));
                        if (damage != before)
                            Debug.Log($"{attacker.Name}: {e.EffectName} изменил урон {before} на {damage}");
                    }

                    // 4) правила типа (уязвимости/иммунитеты) защитника
                    foreach (var rule in defender.TypeRules.OrderBy(e => e.Priority))
                    {
                        int before = damage;
                        damage = Math.Max(0, rule.ApplyTypeRule(ctx, damage));
                        if (damage != before)
                            Debug.Log($"{defender.Name}: {rule.EffectName} изменил урон {before} на {damage}");
                    }

                    // 5) эффекты защиты
                    foreach (var e in defender.Defense.OrderBy(e => e.Priority))
                    {
                        int before = damage;
                        damage = Math.Max(0, e.ModifyIncomingDamage(ctx, damage));
                        if (damage != before)
                            Debug.Log($"{defender.Name}: {e.EffectName} изменил входящий урон {before} на {damage}");
                    }

                    // 5) нанесение урон
                    if (damage > 0)
                    {
                        defender.TakeDamage(damage);
                        Debug.Log($"{attacker.Name} hit {defender.Name} on {damage} → HP {defender.Hp}/{defender.MaxHp}");
                        uiEvents?.OnHit(defender, damage);
                        uiEvents?.OnHpChanged(defender);
                    }
                    else
                    {
                        Debug.Log($"{attacker.Name}didnt cause any damage {defender.Name} (0 dmg after effects)");
                        uiEvents?.OnHit(defender, damage);
                        roundCap = round + 5;
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(stepDelaySeconds));

                    // 6) проверка смерти
                    if (!defender.IsAlive)
                    {
                        var outcome = ReferenceEquals(attacker, hero) ? BattleOutcome.HeroWon : BattleOutcome.HeroDied;
                        var result  = new BattleResult(outcome, round);
                        return result;
                    }
                }

                // 7) смена хода
                (attacker, defender) = (defender, attacker);
            }
            
            // лимит по раундам
            var fallbackOutcome = hero.IsAlive ? BattleOutcome.HeroWon : BattleOutcome.HeroDied;
            var fbResult = new BattleResult(fallbackOutcome, round);
            return fbResult;
        }
    }
}
