using LocalizationSystem.Core;
using UnityEngine;


namespace LocalizationSystem.Integration
{
    public class SetGameLanguage : MonoBehaviour
    {
        [SerializeField] private Language _language;
        public void Set()
        {
            LocalizationManager.Language = _language;
        }
    }

}