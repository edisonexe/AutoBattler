using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Combat.Effects;
using Domain.Core;
using Domain.Combat.Effects.Interfaces;

namespace Domain.Combat
{
    public class BattleManager
    {
        // private readonly Random _rng;
        //
        // public BattleManager(Random rng = null) => _rng = rng ?? new Random();

        public BattleResult Fight(Hero hero, Fighter monster, int roundCap = 200)
        {
            var log = new List<string>();
            hero.ResetForCombat();
            monster.ResetForCombat();

            Fighter attacker = hero.Stats.Agility >= monster.Stats.Agility ? hero : monster;
            Fighter defender = ReferenceEquals(attacker, hero) ? monster : hero;

            int round = 0;
            while (hero.IsAlive && monster.IsAlive && round < roundCap)
            {
                round++;

                // 1) шанс попадания: rnd [1 .. atk.Agi + def.Agi]
                int atkAgi = attacker.Stats.Agility;
                int defAgi = defender.Stats.Agility;
                int roll = Utils.Utils.RandomInt(1, Math.Max(2, atkAgi + defAgi + 1)); // верхняя граница исключена => +1
                bool miss = roll <= defAgi;

                if (miss)
                {
                    log.Add($"[{round}] {attacker.Name} промахнулся по {defender.Name} (roll {roll} ≤ DEF_AGI {defAgi}).");
                }
                else
                {
                    // 2) базовый урон: оружие атакующего + его сила
                    int damage = attacker.GetBaseDamage();

                    // 3) эффекты атаки (по приоритету)
                    var ctx = new EffectContext(attacker, defender);
                    foreach (var eff in attacker.Attack.OfType<IAttackEffect>().OrderBy(e => e.Priority))
                        damage = Math.Max(0, eff.ModifyOutgoingDamage(ctx, damage));

                    // 4) эффекты защиты цели (по приоритету)
                    foreach (var eff in defender.Defense.OfType<IDefenseEffect>().OrderBy(e => e.Priority))
                        damage = Math.Max(0, eff.ModifyIncomingDamage(ctx, damage));

                    // 5) наносим урон
                    if (damage > 0)
                    {
                        defender.TakeDamage(damage);
                        log.Add($"[{round}] {attacker.Name} ударил {defender.Name} на {damage} → HP {defender.Hp}/{defender.MaxHp}");
                    }
                    else
                    {
                        log.Add($"[{round}] {attacker.Name} не причинил урона {defender.Name} (после эффектов 0).");
                    }

                    // 6) проверка смерти
                    if (!defender.IsAlive)
                    {
                        var outcome = ReferenceEquals(attacker, hero) ? BattleOutcome.HeroWon : BattleOutcome.HeroDied;
                        return new BattleResult(outcome, round, log);
                    }
                }

                // 7) смена хода
                (attacker, defender) = (defender, attacker);
            }

            // лимит по раундам
            var fallbackOutcome = hero.IsAlive ? BattleOutcome.HeroWon : BattleOutcome.HeroDied;
            return new BattleResult(fallbackOutcome, round, log);
        }
    }
}
