using System.Threading;
using Cysharp.Threading.Tasks;
using Domain.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Domain.UI
{
    public sealed class BattleView : MonoBehaviour
    {
        [Header("HP UI")]
        [SerializeField] private Image _hpFill;
        [SerializeField] private TMP_Text _hpText;
        
        [Header("Damage Popup")]
        [SerializeField] private TMP_Text _damageText;
        [SerializeField] private float _damageShowTime = 2f;
        private Vector3 _damageMove = new Vector3(0f, 30f, 0f);

        [Header("Info UI")]
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _statsText;
        [SerializeField] private TMP_Text _classLevelsText;
        
        private CancellationTokenSource _damageCts;
        private Vector2 _damageBasePos;
        
        private void Awake()
        {
            _damageBasePos = _damageText.rectTransform.anchoredPosition;
        }
        
        public void BindFighter(Fighter f)
        {
            if (f == null) return;
            if (_nameText)  _nameText.text = f.Name;
            UpdateStats(f);
            UpdateHealth(f);
            UpdateClassLevels(f);
        }
        
        public void UpdateHealth(Fighter f) => UpdateHealth(f.Hp, f.MaxHp);
        
        public void UpdateHealth(int current, int max)
        {
            if (_hpFill) _hpFill.fillAmount = max > 0 ? Mathf.Clamp01((float)current / max) : 0f;
            if (_hpText) _hpText.text = $"{current}/{max}";
        }
        
        public void UpdateStats(Fighter f) => UpdateStats(f.Stats);
        
        public void UpdateStats(Stats s)
        {
            if (!_statsText) return;
            _statsText.text = $"Str {s.Strength}\nAgi {s.Agility}\nSta {s.Stamina}";
        }

        public void UpdateClassLevels(Fighter f)
        {
            if (!_classLevelsText || f is Monster) return;

            if (f is Hero hero && hero.ClassLevels != null)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (var kvp in hero.ClassLevels)
                {
                    sb.AppendLine($"{kvp.Key} - {kvp.Value} lvl");
                }

                _classLevelsText.text = sb.ToString();
            }
        }
        
        public void ShowDamage(int amount, Color? color = null)
        {
            if (!_damageText) return;
            
            _damageCts?.Cancel();
            _damageCts = new CancellationTokenSource();

            _ = DamagePopupAsync(amount, color ?? Color.red, _damageCts.Token);
        }
        
        public void ShowMiss(Color? color = null)
        {
            if (!_damageText) return;

            _damageCts?.Cancel();
            _damageCts = new CancellationTokenSource();

            _ = DamagePopupAsync("miss", color ?? Color.gray, _damageCts.Token);
        }
        
        private UniTaskVoid DamagePopupAsync(int amount, Color color, CancellationToken token) =>
            DamagePopupAsync($"-{Mathf.Max(0, amount)}", color, token);
        
        private async UniTaskVoid DamagePopupAsync(string text, Color color, CancellationToken token)
        {
            _damageText.gameObject.SetActive(true);

            var startPos = _damageBasePos;
            var endPos   = startPos + (Vector2)_damageMove;

            _damageText.rectTransform.anchoredPosition = startPos;
            _damageText.text = text;
            color.a = 1f;
            _damageText.color = color;

            float t = 0f;
            while (t < _damageShowTime)
            {
                if (token.IsCancellationRequested) break;

                t += Time.deltaTime;
                float k = t / _damageShowTime;

                _damageText.rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, k);
                var c = _damageText.color;
                c.a = 1f - k;
                _damageText.color = c;

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            _damageText.gameObject.SetActive(false);
            _damageText.rectTransform.anchoredPosition = startPos;   // вернуть на базовую позицию
        }
    }
}
