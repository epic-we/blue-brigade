using UnityEngine;

namespace LocalizationSystem.Core
{
    public abstract class Localization : MonoBehaviour
    {
        [SerializeField] protected LocalizedInfo[] _localizations;
        private LocalizedInfo _localizedInfo;

        protected virtual void OnEnable()
        {
            UpdateLocalization(LocalizationManager.Language);
            LocalizationManager.onLanguageChange += UpdateLocalization;
        }

		void OnDisable()
		{
            LocalizationManager.onLanguageChange -= UpdateLocalization;
		}

		private void UpdateLocalization(Language language)
        {
            _localizedInfo = LocalizedInfo.GetLocalization(language, _localizations, this);
            Localize(_localizedInfo);
        }

        protected abstract void Localize(LocalizedInfo info);
    }
}