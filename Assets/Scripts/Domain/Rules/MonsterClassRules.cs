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
                    monster.AddTypeRule(new SkeletonEffect());
                    break;
                
                case MonsterClass.Slime:
                    monster.AddTypeRule(new SlimeEffect());
                    break;
                
                case MonsterClass.Ghost:
                    monster.AddAttackEffect(new HiddenAttack());
                    break;
                
                case MonsterClass.Golem:
                    monster.AddDefenseEffect(new StoneSkin());
                    break;

                case MonsterClass.Dragon:
                    monster.AddAttackEffect(new DragonEffect());
                    break;
                
            }
        }
    }
}