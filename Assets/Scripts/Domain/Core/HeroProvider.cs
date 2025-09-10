using UnityEngine;

namespace Domain.Core
{
    public class HeroProvider
    {
        public Hero Current { get; private set; }
        public void Set(Hero hero)
        {
            if (hero == null)
            {
                Debug.Log("HeroProvider: герой сброшен в null");
            }
            else
            {
                Debug.Log($"HeroProvider: установлен герой");
                hero.PrintInfoAboutFighter();
            }

            Current = hero;
        }
    }

}