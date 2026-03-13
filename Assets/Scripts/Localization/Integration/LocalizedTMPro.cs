using LocalizationSystem.Core;
using TMPro;
using UnityEngine;

namespace LocalizationSystem.Integration
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedTMPro : Localization
    {
        private TMP_Text _tmPro;

        [Header("Layout tweak (optional)")]
        [SerializeField] private RectTransform targetToMove;   
        [SerializeField] private float ptMoveX = 0f;           

        private bool _cachedDefault;
        private Vector2 _defaultAnchoredPos;

        private void Awake()
        {
            _tmPro ??= GetComponent<TMP_Text>();

         
            if (targetToMove == null)
                targetToMove = GetComponent<RectTransform>();

            CacheDefaultPos();
        }

        private void CacheDefaultPos()
        {
            if (_cachedDefault || targetToMove == null) return;
            _defaultAnchoredPos = targetToMove.anchoredPosition;
            _cachedDefault = true;
        }

        protected override void Localize(LocalizedInfo info)
        {
            _tmPro.text = info.Text;

            CacheDefaultPos();

            if (targetToMove == null || !_cachedDefault)
                return;

            
            targetToMove.anchoredPosition = _defaultAnchoredPos;

           
            if (info.Language == Language.pt_pt)
            {
                targetToMove.anchoredPosition = _defaultAnchoredPos + new Vector2(ptMoveX, 0f);
            }
        }

        private void Reset()
        {
            _tmPro = GetComponent<TMP_Text>();
            targetToMove = GetComponent<RectTransform>();

            if (_tmPro)
            {
                _localizations = new LocalizedInfo[1];
                _localizations[0] = new LocalizedInfo(Language.pt_pt, _tmPro.text);
            }
        }
    }
}