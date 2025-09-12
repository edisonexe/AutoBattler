using Domain.Combat.Effects.Hero;
using Domain.Combat.Effects.Monster;
using Domain.Core;

namespace Domain.Rules
{
    public class MonsterClassRules
    {
        public static void Apply(Monster monster, MonsterClass cls)
        {
            switch (cls)
            {
                case MonsterClass.Skeleton:
                    monster.TypeRules.Add(new SkeletonEffect());
                    break;
                
                case MonsterClass.Slime:
                    monster.TypeRules.Add(new SlimeEffect());
                    break;
                
                case MonsterClass.Ghost:
                    monster.Attack.Add(new HiddenAttack());
                    break;
                
                case MonsterClass.Golem:
                    monster.Defense.Add(new StoneSkin());
                    break;

                case MonsterClass.Dragon:
                    monster.Attack.Add(new DragonEffect());
                    break;
                
            }
        }
    }
}