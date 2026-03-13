using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TalkingBubble : MonoBehaviour
{

    public CreateTopics topicManager;
    private Sprite topicSprite;
    private Transform player;
    [SerializeField] private GameObject bubbleParent;
    [SerializeField] private float distanceToSeeConversation = 5f;
    [SerializeField] private GameObject cantSeeSprite;
    [SerializeField] private SpriteRenderer symbolPlace;

    [SerializeField] private SpriteRenderer hatSprite;
    private Sprite hat;

    private bool talking = false;
    //private bool topicIsFarAway = true;
    public bool badTopic = false;

    public bool badHat = false;
    public bool badSinging = false;
    public bool badRadio = false;

    [SerializeField] private GameObject radio;
    [SerializeField] private GameObject book;
    [SerializeField] private SpriteRenderer bookCover;
    [SerializeField] private SpriteRenderer bookSymbol;
    [SerializeField] private SpriteRenderer bookOutline;
    private (Sprite, Color) bookObject;
    public bool badBook = false;
    public CivilianFaultType _civilianFaultType;

    [field: Header("Talking animations")]
    [SerializeField] private float talkingPopUpAnimation = 0.3f;
    [SerializeField] private float timeSpentTalking = 4f;
    [SerializeField] private float timeTalkingRandomOffset = 1f;
    private float currentTimeToTalk = 0f;
    [SerializeField] private float talkingCooldown = 1f;
    private float currentTalkingCooldown = 0f;

    private Vector3 initialbubbleScale = Vector3.one;

    private Coroutine bubblePopCoroutine = null;

    [SerializeField] private int chanceToWhistle = 10;
    [SerializeField] private GameObject whistleParent;
    [SerializeField] private GameObject whistlePrefab;

    [SerializeField] private AudioClip[] convoSounds;

    private bool Censored { get; set; } = false;

    private bool singing = false;

    public void Initialize(CivilianFaultType type)
    {
        _civilianFaultType = type;
        topicManager = FindAnyObjectByType<CreateTopics>();
        player = FindAnyObjectByType<PlayerMovement>().gameObject.transform;
        GetRandomHat(type == CivilianFaultType.Fashion, topicManager.specialDay);
        GetRandomBook(type == CivilianFaultType.Item, topicManager.specialDay);
        GetRandomTopic(type == CivilianFaultType.Talking, topicManager.specialDay);

        //if(Random.Range(0, 100) < chanceToWhistle)
        if (type == CivilianFaultType.Singing)
        {
            badSinging = true;
            Instantiate(whistlePrefab, whistleParent.transform);
        }
        else
        {
            //This is for some NPCs to sing even on days where singing is not censurable
            if (topicManager.GetRandomSinging() && Random.Range(0, 100) < chanceToWhistle)
            {
                Instantiate(whistlePrefab, whistleParent.transform);
            }
        }

        if (type == CivilianFaultType.Radio)
        {
            badRadio = true;
            ActivateRadio();
            book.SetActive(false);
        }
        else
        {
            //This is for some NPCs to carry radios even on days where radios are not censurable
            if (topicManager.GetRandomRadio() && Random.Range(0, 100) < chanceToWhistle && bookObject.Item1 == null)
            {
                ActivateRadio();
            }
        }

        InitalizerSpeakingBubble();
    }

    private void InitalizerSpeakingBubble()
    {
        initialbubbleScale = bubbleParent.transform.localScale;
        bubbleParent.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) { }
    }

    private void FixedUpdate()
    {
        if (talking)
            if (Vector2.Distance(player.position, transform.position) > distanceToSeeConversation)// && !topicIsFarAway)
            {
                if (cantSeeSprite != null)
                {
                    symbolPlace.gameObject.SetActive(false);
                    cantSeeSprite.SetActive(true);
                }
                //topicIsFarAway = true;
            }
            else if (Vector2.Distance(player.position, transform.position) < distanceToSeeConversation)// && topicIsFarAway)
            {
                if (topicSprite != null)
                {
                    symbolPlace.gameObject.SetActive(true);
                    cantSeeSprite.SetActive(false);
                }
                //topicIsFarAway = false;
            }
    }

    public void GetRandomHat(bool badHatToggle, bool special = false)
    {
        (hat, badHat) = topicManager.GetRandomHat(badHatToggle, special);

        hatSprite.sprite = hat;
    }

    public void GetRandomBook(bool badBookToggle, bool special = false)
    {
        (bookObject, badBook) = topicManager.GetRandomBook(badBookToggle, special);

        if (bookObject.Item1 == null)
            return;

        book.SetActive(true);

        if(special)
        {
            //bookOutline.gameObject.SetActive(false);
            //bookCover.gameObject.SetActive(false );
            bookSymbol.color = bookObject.Item2;
            bookSymbol.sprite = bookObject.Item1;
        }
        else
        {
            //bookOutline.gameObject.SetActive(true);
            //bookCover.gameObject.SetActive(true);
            bookCover.color = bookObject.Item2;
            bookSymbol.color = bookObject.Item2;
            bookSymbol.sprite = bookObject.Item1;
        }

    }

    private void ActivateRadio()
    {
        radio.SetActive(true);
    }


    public void GetRandomTopic(bool badTopicToggle, bool special = false)
    {
        (topicSprite, badTopic) = topicManager.GetRandomTopic(badTopicToggle, special);
        //Debug.Log("talking: " + topicSprite.name);
       
        symbolPlace.sprite = topicSprite;
    }

    public void StartTalking()
    {
        //Don't talk if censored or if disappeared in last level
        if (Censored)
        {
            return;
        }

        whistleParent.SetActive(false);
        talking = true;
        bubbleParent.SetActive(true);

        if (bubblePopCoroutine != null)
            StopCoroutine(bubblePopCoroutine);

        bubblePopCoroutine = StartCoroutine(TalkingAnimation());
    }

    private IEnumerator TalkingAnimation()
    {
        currentTimeToTalk = timeSpentTalking + Random.Range(-timeTalkingRandomOffset, timeTalkingRandomOffset);

        float lerpValue = 0;
        

        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime / talkingPopUpAnimation;
            bubbleParent.transform.localScale = Vector3.Lerp(Vector3.zero, initialbubbleScale, lerpValue);
            yield return null;
        }
        AudioSystem.PlaySound(convoSounds, 0.3f);
        yield return new WaitForSeconds(currentTimeToTalk);

        lerpValue = 0;

        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime / talkingPopUpAnimation;
            bubbleParent.transform.localScale = Vector3.Lerp(initialbubbleScale, Vector3.zero, lerpValue);
            yield return null;
        }

        currentTalkingCooldown = talkingCooldown + Random.Range(-0.5f, 0.5f);

        yield return new WaitForSeconds(currentTalkingCooldown);

        bubblePopCoroutine = StartCoroutine(TalkingAnimation());
    }

    public void StopTalking()
    {
        Censored = true;
        whistleParent.SetActive(false);

        talking = false;
        bubbleParent.SetActive(false);

        if(bubblePopCoroutine != null)
            StopCoroutine(bubblePopCoroutine);

        bubbleParent.transform.localScale = initialbubbleScale;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, distanceToSeeConversation);
    }
}
