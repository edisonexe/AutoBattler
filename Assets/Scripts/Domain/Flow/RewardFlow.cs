using Cysharp.Threading.Tasks;
using Domain.Core;
using Domain.UI;

namespace Domain.Flow
{
    public class RewardFlow
    {
        private readonly WeaponSelectionView _weaponSelection;

        public RewardFlow(WeaponSelectionView weaponSelection)
        {
            _weaponSelection = weaponSelection;
        }

        /// Предлагает награду, если она есть. Возвращает true, если окно показывалось (была награда).
        public async UniTask<bool> TryOfferWeaponAsync(Hero hero, Weapon reward)
        {
            if (_weaponSelection == null || reward == null)
                return false;

            var tcs = new UniTaskCompletionSource<bool>();

            void Cleanup()
            {
                _weaponSelection.OnTake -= OnTake;
                _weaponSelection.OnSkip -= OnSkip;
            }

            void OnTake(Weapon w)
            {
                if (w != null) hero.SetWeapon(w);
                Cleanup();
                tcs.TrySetResult(true);
            }

            void OnSkip()
            {
                Cleanup();
                tcs.TrySetResult(true);
            }

            _weaponSelection.OnTake += OnTake;
            _weaponSelection.OnSkip += OnSkip;
            _weaponSelection.Show(hero.Weapon, reward);

            await tcs.Task;
            return true;
        }
    }
}