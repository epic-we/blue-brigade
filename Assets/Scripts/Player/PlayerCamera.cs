using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _cameraSpeed = 5;

    private float _lockedZ;

    private void Awake()
    {
        _lockedZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = _target.transform.position;
        targetPos.z = 0;
        Vector3 newPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _cameraSpeed);
        newPos.z = _lockedZ;
        transform.position = newPos;
    }
}
