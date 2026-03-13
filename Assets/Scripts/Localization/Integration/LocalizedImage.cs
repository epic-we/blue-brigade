using LocalizationSystem.Core;
using UnityEngine;
using UnityEngine.UI;

namespace LocalizationSystem.Integration
{
    [RequireComponent(typeof(Image))]
    public class LocalizedImage : Localization
    {
        private Image _i;

        private void Awake()
        {
            _i ??= GetComponent<Image>();
        }

        protected override void Localize(LocalizedInfo info)
        {
            _i.sprite = info.Sprite;
        }

        void Reset()
        {
            _i = GetComponent<Image>();
            if (_i)
            {
                _localizations = new LocalizedInfo[1];
                _localizations[0] = new LocalizedInfo(Language.pt_pt, _i.sprite);
            }
        }
    }
}