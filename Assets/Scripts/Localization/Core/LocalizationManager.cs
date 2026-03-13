using System;

namespace LocalizationSystem.Core
{
    // Very simple, rough and slightly scalable (in low quantities) Localization system for Blue Brigade
    public static class LocalizationManager
    {
        public static Language Language
        {
            get => _lang;
            set
            {
                if (_lang == value)
                    return;
                _lang = value;
                onLanguageChange?.Invoke(value);
            }
        }
        private static Language _lang;

        static LocalizationManager()
        {
            Language = default;
        }

        public static event Action<Language> onLanguageChange;
    }

}