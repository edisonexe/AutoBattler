using UnityEngine;

namespace Domain.Core
{
    public class HeroProvider
    {
        public Hero Current { get; private set; }
        public void Set(Hero hero)
        {
            Current = hero;
        }
    }

}