using UnityEngine;

public class YSort : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _maiSr;
    [SerializeField] private float _yOffset = 0;
    [SerializeField] private bool _doOnStart = false;
    [SerializeField] private SpriteRenderer[] _extraSpriteRenderers;

    private void Start()
    {
        if (_doOnStart)
        {
            Sort();
            Destroy(this);
            return;
        }
    }

    private void LateUpdate()
    {
        Sort();
    }

    private void Sort()
    {
        // Update Y sorting
        float newOrderZ = (transform.position.y + _yOffset) * .001f;
        Vector3 newPos = _maiSr.transform.position;
        newPos.z = newOrderZ;
        _maiSr.transform.position = newPos;
        foreach (SpriteRenderer sr in _extraSpriteRenderers)
        {
            newPos = sr.transform.position;
            newPos.z = newOrderZ - 0.00001f;
            sr.transform.position = newPos;
        }
    }

    private void Reset()
    {
        _maiSr = GetComponent<SpriteRenderer>();
    }
}
