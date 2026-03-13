using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private KeyCode up = KeyCode.W;
    [SerializeField] private KeyCode down = KeyCode.S;
    [SerializeField] private KeyCode left = KeyCode.A;
    [SerializeField] private KeyCode right = KeyCode.D;
    [SerializeField] private KeyCode alternativeUp = KeyCode.UpArrow;
    [SerializeField] private KeyCode alternativeDown = KeyCode.DownArrow;
    [SerializeField] private KeyCode alternativeLeft = KeyCode.LeftArrow;
    [SerializeField] private KeyCode alternativeRight = KeyCode.RightArrow;
    [SerializeField] private KeyCode slap = KeyCode.Space;

    [SerializeField] private float movSped = 5f;
    private Vector2 velocity = Vector2.zero;

    [SerializeField]
    private Vector3 initialPosition;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip[] _pencilHitGroundClips;
    private AudioSource audioSource;

    private bool lastSlap = true;
    private bool movEnabled = true;
    private bool lastSideMovementRight = true;
    public bool CanPlaySound { get; set; } = true;

    private bool firstMovementDetected = false; // to track first player input

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        velocity = Vector2.zero;

        if (movEnabled)
        {
            if (Input.GetKey(up) || Input.GetKey(alternativeUp))
            {
                velocity.y = 1;
            }

            if (Input.GetKey(down) || Input.GetKey(alternativeDown))
            {
                velocity.y = -1;
            }

            if (Input.GetKey(right) || Input.GetKey(alternativeRight))
            {
                velocity.x = 1;
                LastSideMovement();
            }

            if (Input.GetKey(left) || Input.GetKey(alternativeLeft))
            {
                velocity.x = -1;
                LastSideMovement();
            }

            // Detect first movement
            if (!firstMovementDetected && velocity != Vector2.zero)
            {
                firstMovementDetected = true;
                // Previously we would create a checkpoint here, now it's removed
            }
        }

        rb.linearVelocity = velocity.normalized * movSped;
        UpdateAnimation();
    }

    private void LastSideMovement()
    {
        if (velocity.x < 0)
        {
            lastSideMovementRight = false;
        }
        else if (velocity.x > 0)
        {
            lastSideMovementRight = true;
        }
    }

    private void UpdateAnimation()
    {
        bool walking = rb.linearVelocity.magnitude > 0.1f;
        _animator.SetBool("walk", walking);

        if ((Input.GetKeyDown(slap) || Input.GetMouseButtonDown(0)) && lastSlap && CanPlaySound)
        {
            _animator.SetTrigger("slap");
            AudioClip clip = _pencilHitGroundClips[Random.Range(0, _pencilHitGroundClips.Length)];
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(clip);

            if (!movEnabled)
            {
                lastSlap = false;
            }
        }

        if (walking)
        {
            if (lastSideMovementRight)
                _animator.transform.right = Vector2.right;
            else
                _animator.transform.right = Vector2.left;
        }
    }

    public void StartMoving(bool canMove)
    {
        movEnabled = canMove;

        if (canMove)
        {
            lastSlap = canMove;
        }
        else
        {
            StartCoroutine(LastSlapCR());
        }
    }

    private IEnumerator LastSlapCR()
    {
        yield return new WaitForSeconds(0.1f);
        lastSlap = false;
    }

    public void DelayStartMovement()
    {
        StartCoroutine(DelayMovementCoroutine(1f));
    }

    public void DelayStartMovementFromTitle()
    {
        StartCoroutine(DelayMovementCoroutine(0.2f));
    }

    private IEnumerator DelayMovementCoroutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        StartMoving(true);
        CanPlaySound = true;
    }



    public void ResetPosition()
    {
        transform.position = initialPosition;
    }
}
