using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Domain.Combat.Effects;
using Domain.Core;
using Domain.UI.Interfaces;

namespace Domain.Combat
{
    public class BattleManager
    {
        public async UniTask<BattleResult> FightAsync(Hero hero, Fighter monster, float stepDelaySeconds = 2f,
            int roundCap = 200, IUIEvents uiEvents = null)
        {
            var log = new List<string>();
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
                
                // 0) урон в начале хода от атакующего к защитнику
                var ctx = new EffectContext(attacker, defender);
                int startTurnDamage = attacker.StartTurn
                    .OrderBy(e => e.Priority)
                    .Sum(e => Math.Max(0, e.AddStartTurnDamage(ctx)));

                if (startTurnDamage > 0)
                {
                    defender.TakeDamage(startTurnDamage);
                    uiEvents?.OnHit(attacker, defender, startTurnDamage, round);
                    uiEvents?.OnHpChanged(defender);
                    if (!defender.IsAlive)
                    {
                        var outcome = ReferenceEquals(defender, hero) ? BattleOutcome.HeroDied : BattleOutcome.HeroWon;
                        return new BattleResult(outcome, round, log);
                    }
                }
                
                
                // 1) шанс попадания: rnd [1 .. atk.Agi + def.Agi]
                int atkAgi = attacker.Stats.Agility;
                int defAgi = defender.Stats.Agility;
                int roll   = Utils.Utils.RandomInt(1, Math.Max(2, atkAgi + defAgi));
                bool miss  = roll <= defAgi;

                if (miss)
                {
                    log.Add($"[{round}] {attacker.Name} промахнулся по {defender.Name} (roll {roll} ≤ DEF_AGI {defAgi}).");
                    uiEvents?.OnMiss(attacker, defender, roll, defAgi, round);
                    await UniTask.Delay(TimeSpan.FromSeconds(stepDelaySeconds));
                }
                else
                {
                    // 2) базовый урон: оружие атакующего + его сила
                    int damage = attacker.GetBaseDamage();


                    // var ctx = new EffectContext(attacker, defender);
                    // 3) эффекты атаки
                    foreach (var eff in attacker.Attack.OrderBy(e => e.Priority))
                        damage = Math.Max(0, eff.ModifyOutgoingDamage(ctx, damage));

                    // 4) правила типа (уязвимости/иммунитеты) защитника
                    foreach (var rule in defender.TypeRules.OrderBy(e => e.Priority))
                        damage = Math.Max(0, rule.ApplyTypeRule(ctx, damage));

                    // 5) эффекты защиты
                    foreach (var eff in defender.Defense.OrderBy(e => e.Priority))
                        damage = Math.Max(0, eff.ModifyIncomingDamage(ctx, damage));

                    // 5) нанесение урон
                    if (damage > 0)
                    {
                        defender.TakeDamage(damage);
                        log.Add($"[{round}] {attacker.Name} ударил {defender.Name} на {damage} → HP {defender.Hp}/{defender.MaxHp}");
                        uiEvents?.OnHit(attacker, defender, damage, round);
                        uiEvents?.OnHpChanged(defender);
                    }
                    else
                    {
                        log.Add($"[{round}] {attacker.Name} не причинил урона {defender.Name} (после эффектов 0).");
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(stepDelaySeconds));

                    // 6) проверка смерти
                    if (!defender.IsAlive)
                    {
                        var outcome = ReferenceEquals(attacker, hero) ? BattleOutcome.HeroWon : BattleOutcome.HeroDied;
                        var result  = new BattleResult(outcome, round, log);
                        return result;
                    }
                    
                    // if (!defender.IsAlive)
                    // {
                    //     var outcome = ReferenceEquals(attacker, hero) ? BattleOutcome.HeroWon : BattleOutcome.HeroDied;
                    //     return new BattleResult(outcome, round, log);
                    // }
                }

                // 7) смена хода
                (attacker, defender) = (defender, attacker);
            }
            
            // лимит по раундам
            var fallbackOutcome = hero.IsAlive ? BattleOutcome.HeroWon : BattleOutcome.HeroDied;
            var fbResult = new BattleResult(fallbackOutcome, round, log);
            return fbResult;
            
            // var fallbackOutcome = hero.IsAlive ? BattleOutcome.HeroWon : BattleOutcome.HeroDied;
            // return new BattleResult(fallbackOutcome, round, log);
        }
    }
}
