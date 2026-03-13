using UnityEngine;

public class VelocityX : MonoBehaviour
{

[SerializeField] private float movSpeedX = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocityX = movSpeedX;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
