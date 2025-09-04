using System;
using System.Text;
using Domain.Core;

namespace Domain.Combat
{
    public class CombatResolver
    {
        private Random _rnd =  new Random();
        
        public class Result
        {
            public bool HeroWon;
            public string Log;
        }

        public Result Simulate(Hero hero, Monster monster)
        {
            var sb = new StringBuilder();

            // Подготовка
            hero.ResetForCombat();
            monster.ResetForCombat();

            // Кто ходит первым: по ловкости; при равенстве — герой
            Fighter attacker = hero.Stats.Agility >= monster.Stats.Agility ? (Fighter)hero : monster;
            Fighter defender = attacker == hero ? (Fighter)monster : hero;

            sb.AppendLine($"Начало боя: {hero.Name} vs {monster.Name}");
            sb.AppendLine($"Первым ходит: {attacker.Name}");

            // Основной цикл
            while (hero.IsAlive && monster.IsAlive)
            {
                sb.AppendLine($"\nХод: {attacker.Name}");

                // 1) бросок на попадание
                int rollMax = attacker.Stats.Agility + defender.Stats.Agility;
                int roll = _rnd.Next(1, rollMax + 1); // включительно
                sb.AppendLine($"  Бросок на попадание: {roll} (1..{rollMax}), Ловк цели = {defender.Stats.Agility}");

                bool miss = roll <= defender.Stats.Agility; // если выпало <= ловкости цели — промах
                if (miss)
                {
                    sb.AppendLine("  Промах!");
                }
                else
                {
                    // 2) базовый урон = урон оружия + сила
                    int damage = attacker.GetBaseDamage();
                    damage = Math.Max(0, damage);

                    if (damage > 0)
                    {
                        defender.TakeDamage(damage);
                        sb.AppendLine($"  Попадание на {damage} урона. HP {defender.Name}: {defender.Hp}/{defender.MaxHp}");
                    }
                    else
                    {
                        sb.AppendLine("  Урон не прошёл.");
                    }
                }

                if (!defender.IsAlive)
                    break;

                attacker.IncrementTurn();

                // смена сторон
                (attacker, defender) = (defender, attacker);
            }

            bool heroWon = hero.IsAlive;
            sb.AppendLine(heroWon ? "\nПобеда героя!" : "\nГерой проиграл...");

            return new Result { HeroWon = heroWon, Log = sb.ToString() };
        }
    }
}
