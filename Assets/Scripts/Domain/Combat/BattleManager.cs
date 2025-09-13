using System;
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
            var ctx = new EffectContext(attacker, defender);
            await UniTask.Delay(TimeSpan.FromSeconds(stepDelaySeconds));
            
            int round = 0;
            while (hero.IsAlive && monster.IsAlive && round < roundCap)
            {
                round++;
                attacker.IncrementTurn();
                Debug.Log($"[{attacker.Name}: turn {attacker.TurnsTaken}]");

                ctx.Attacker = attacker;
                ctx.Defender = defender;
                
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
                    var atkEffects = attacker.Attack;
                    for (int i = 0; i < atkEffects.Count; i++)
                    {
                        int before = damage;
                        damage = Math.Max(0, atkEffects[i].ModifyOutgoingDamage(ctx, damage));
                        if (damage != before)
                            Debug.Log($"{attacker.Name}: {atkEffects[i].EffectName} изменил урон {before} на {damage}");
                    }

                    // 4) правила типа (уязвимости/иммунитеты) защитника
                    var typeEffects = defender.TypeRules;
                    for (int i = 0; i < typeEffects.Count; i++)
                    {
                        int before = damage;
                        damage = Math.Max(0, typeEffects[i].ApplyTypeRule(ctx, damage));
                        if (damage != before)
                            Debug.Log($"{defender.Name}: {typeEffects[i].EffectName} изменил урон {before} на {damage}");
                    }

                    // 5) эффекты защиты
                    var defEffects = defender.Defense;
                    for (int i = 0; i < defEffects.Count; i++)
                    {
                        int before = damage;
                        damage = Math.Max(0, defEffects[i].ModifyIncomingDamage(ctx, damage));
                        if (damage != before)
                            Debug.Log($"{defender.Name}: {defEffects[i].EffectName} изменил входящий урон {before} на {damage}");
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
