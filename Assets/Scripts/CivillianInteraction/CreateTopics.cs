using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.ProbeAdjustmentVolume;


public class CreateTopics : MonoBehaviour
{

    [SerializeField, Header("BOOKS")] private TalkingData data;
    [SerializeField, Range(0, 100), Header("TOPICS")] private int chanceOfFootballTopic = 30;
    private int winningFootBallTeam = 0;
    private int losingFootBallTeam = 1;
    private List<TalkingData.TalkingTopics> goodTopics;
    private List<TalkingData.TalkingTopics> tempGoodTopics;
    public List<TalkingData.TalkingTopics> GoodTopics => goodTopics;
    private List<TalkingData.TalkingTopics> badTopics;
    public List<TalkingData.TalkingTopics> BadTopics => badTopics;
    [SerializeField] private int numberOfBadTopics = 2;
    [Header("HATS")]
    private List<TalkingData.TalkingTopics> goodHats;
    private List<TalkingData.TalkingTopics> tempGoodHats;
    public List<TalkingData.TalkingTopics> GoodHats => goodHats;
    private List<TalkingData.TalkingTopics> badHats;
    public List<TalkingData.TalkingTopics> BadHats => badHats;
    [SerializeField] private int numberOfBadHats = 2;

    [SerializeField, Range(0, 100), Header("BOOKS")] private int chanceOfBook = 30;
    [SerializeField, Range(1, 3)] private int numberOfColors = 3;
    private List<(TalkingData.BookShapes, Color)> books;
    private List<(TalkingData.BookShapes, Color)> tempBooks;
    public List<(TalkingData.BookShapes, Color)> Books => books;
    private List<(TalkingData.BookShapes, Color)> badBooks;
    public List<(TalkingData.BookShapes, Color)> BadBooks => badBooks;
    [SerializeField] private int numberOfBadBooks = 2;

    [Header("UI")]
    [SerializeField] private Image[] modelsForHats;
    [SerializeField] private Sprite[] spritesModelsForHats;
    [SerializeField] private Image forbiddenSingingImage;
    [SerializeField] private Image forbiddenRadioImage;
    [SerializeField] private Image[] forbiddenWordsImages;
    private int currentForbiddenWord = 0;

    [SerializeField] private Image[] forbiddenHatsImages;
    private int currentForbiddenHat = 0;

    [SerializeField] private Image censorBar;
    [SerializeField] private Image[] forbiddenBookImages;
    [SerializeField] private Image[] forbiddenBookCovers;
    [SerializeField] private Image[] forbiddenBookShapes;
    private int currentForbiddenBook = 0;

    [SerializeField] private Image winningTeam;
    [SerializeField] private Image losingTeam;

    public Animator badgeAnimator;
    public GameObject badgeCover1;
    public GameObject badgeCover2;
    public GameObject badgeCover3;

    public GameObject goodJobObject;
    public GameObject revolutionObject;
    public GameObject timeRanOutObject;
    public GameObject censoringInnocentsObject;

    public bool specialDay = false;

    private bool badSinging = false;
    private bool badRadio = false;
    private bool badFootball = false;

    private int numberOfCensoredThings = 0;
    [SerializeField]
    private Image[] allCensorImages;
    private List<(Sprite, Sprite)> censoredSprites;
    private List<(Sprite, Sprite)> currentCensoredSprites;
    private List<(Sprite, Sprite)> spritesToDelete;
    private List<Sprite> newPolicemanCensoredSprites;
    [SerializeField]
    private Image[] newPolicemanCensoredImages;
    private int imagesLevel = -1;

    [SerializeField]
    private int numberOfPossibleBadItems = 0;
    [SerializeField]
    private int numberOfPossibleBadHats = 0;

    private List<TalkingData.BookShapes> possibleBadItems;
    private List<TalkingData.TalkingTopics> possibleBadHats;
    private List<TalkingData.TalkingTopics> possibleBadTopics;

    private int newTopics = 0;
    private int newBooks = 0;
    private int newHats = 0;
    private int newSinging = 0;
    private int newRadio = 0;
    private List<TalkingData.TalkingTopics> newBadTopics;
    private List<TalkingData.TalkingTopics> newBadHats;
    private List<(TalkingData.BookShapes, Color)> newBadBooks;


    private bool cloveSpawn = false;

    public void ActivateSpecialDay()
    {
        specialDay = true;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //CreateNewTopics();
        //CreateNewHats();
        //CreateNewBooks();
    }

    public void SetNumberOfFaultTypes(int amountBadTopics, int newTopics, int amountBadHats, int newHats, int amountOfBadBooks, int newBooks, bool singing, int newSinging, bool radio, int newRadio, bool football, bool resetSprites)
    {
        numberOfCensoredThings = 0;
        numberOfBadTopics = amountBadTopics;
        numberOfBadHats = amountBadHats;
        numberOfBadBooks = amountOfBadBooks;
        numberOfCensoredThings += amountBadTopics + amountBadHats + amountOfBadBooks;
        if (singing) { numberOfCensoredThings++; }
        if (radio) { numberOfCensoredThings++; }
        this.newTopics = newTopics;
        this.newBooks = newBooks;
        this.newHats = newHats;
        this.newSinging = newSinging;
        this.newRadio = newRadio;

        badFootball = football;
        if (football) {
            numberOfCensoredThings++;
        }

        if (resetSprites)
        {
            censoredSprites = new List<(Sprite, Sprite)>();
        }
        currentCensoredSprites = new List<(Sprite, Sprite)>();
        spritesToDelete = new List<(Sprite, Sprite)>();

        newBadTopics = new List<TalkingData.TalkingTopics>();
        newBadHats = new List<TalkingData.TalkingTopics> ();
        newBadBooks = new List<(TalkingData.BookShapes, Color)> ();
        cloveSpawn = false;
        newPolicemanCensoredSprites = new List<Sprite>();
    }

    public void IncreaseCloves()
    {
        cloveSpawn = true;
    }

    public void CreateNewBooks()
    {
        currentForbiddenBook = 0;

        books = new List<(TalkingData.BookShapes, Color)>();
        tempBooks = new List<(TalkingData.BookShapes, Color)>();
        badBooks = new List<(TalkingData.BookShapes, Color)>();
        newBadBooks = new List<(TalkingData.BookShapes, Color)>();

        foreach (TalkingData.BookShapes shape in data.bookShapes)
        {
            for (int i = 0; i < numberOfColors; i++)
            {
                books.Add((shape, data.bookColors[i]));
            }
        }

        foreach (TalkingData.BookShapes shape in possibleBadItems)
        {
            for (int i = 0; i < numberOfColors; i++)
            {
                tempBooks.Add((shape, data.bookColors[i]));
            }
        }

        for (int i = 0; i < numberOfBadBooks; i++)
        {
            //int randomBadBook = Random.Range(0, books.Count);
            //(Sprite, Color) badBook = books[randomBadBook];
            (TalkingData.BookShapes, Color) badBook = tempBooks[i];
            books.Remove(badBook);
            badBooks.Add(badBook);
            if (i >= numberOfBadBooks - newBooks)
            {
                newBadBooks.Add(badBook);
            }
            //forbiddenBookImages[currentForbiddenBook].gameObject.SetActive(true);
            //forbiddenBookImages[currentForbiddenBook].color = badBook.Item2;
            //forbiddenBookShapes[currentForbiddenBook].color = badBook.Item2;
            //forbiddenBookShapes[currentForbiddenBook].sprite = badBook.Item1;
            if (!censoredSprites.Contains((badBook.Item1.book, badBook.Item1.uiBook)))
            {
                censoredSprites.Add((badBook.Item1.book, badBook.Item1.uiBook));
                newPolicemanCensoredSprites.Add(badBook.Item1.uiBook);
            }
            currentCensoredSprites.Add((badBook.Item1.book, badBook.Item1.uiBook));
            currentForbiddenBook++;
        }

        if (specialDay)
        {
            Debug.Log("Special day");
            ((Sprite, Sprite), Color) badBook = ((data.specialItem.symbol, data.specialItem.uiSymbol), Color.white);
            //forbiddenBookImages[currentForbiddenBook].gameObject.SetActive(true);
            //forbiddenBookImages[currentForbiddenBook].sprite = badBook.Item1;
            //forbiddenBookCovers[currentForbiddenBook].gameObject.SetActive(false);
            //forbiddenBookShapes[currentForbiddenBook].gameObject.SetActive(false);
            if (!censoredSprites.Contains(badBook.Item1))
            {
                censoredSprites.Add(badBook.Item1);
                newPolicemanCensoredSprites.Add(badBook.Item1.Item2);
            }
            currentCensoredSprites.Add(badBook.Item1);
            currentForbiddenBook++;
        }

        for (int i = currentForbiddenBook; i < forbiddenBookImages.Length; i++)
        {
            forbiddenBookShapes[i].gameObject.SetActive(false);
        }

        //Debug.Log("currentForbiddenBook" + currentForbiddenBook);

        currentForbiddenBook = 0;
    }       

    public ((Sprite, Color), bool) GetRandomBook(bool badBookToggle, bool special = false)
    {

        if (special && badBookToggle)
        {
            return ((data.specialItem.symbol, Color.white), true);
        }

        if (!badBookToggle)
        {
            int bookRNG = Random.Range(0, 100);

            if (bookRNG > chanceOfBook)
                return ((null, Color.white), false);

            int randomBook = Random.Range(0, books.Count);

            (Sprite, Color) goodBook = (books[randomBook].Item1.book, books[randomBook].Item2);
            return (goodBook, false);
        }
        else
        {
            (Sprite, Color) badBook;

            if (cloveSpawn)
            {
                return ((data.specialItem.symbol, Color.white), true);
            }

            if (newBadBooks.Count > 0)
            {
                int randomBook = Random.Range(0, newBadBooks.Count);
                // Bad topic
                badBook = (newBadBooks[randomBook].Item1.book, newBadBooks[randomBook].Item2);
                newBadBooks.RemoveAt(randomBook);
                return (badBook, true);
            }
            else
            {
                int randomBook = Random.Range(0, badBooks.Count);
                badBook = (badBooks[randomBook].Item1.book, badBooks[randomBook].Item2);
                return (badBook, true);
            }
        }

    }

    public void CreateNewHats()
    {

        currentForbiddenHat = 0;

        goodHats = new List<TalkingData.TalkingTopics>();
        tempGoodHats = new List<TalkingData.TalkingTopics>();
        badHats = new List<TalkingData.TalkingTopics>();



        goodHats = data.hats.OfType<TalkingData.TalkingTopics>().ToList();
        tempGoodHats = possibleBadHats;

        for (int i = 0; i < numberOfBadHats; i++)
        {
            //int randomBadHat = Random.Range(0, goodHats.Count);
            //TalkingData.TalkingTopics badHat = goodHats[randomBadHat];
            TalkingData.TalkingTopics badHat = tempGoodHats[i];
            goodHats.Remove(badHat);
            badHats.Add(badHat);
            if (i >= numberOfBadHats - newHats)
            {
                newBadHats.Add(badHat);
            }
            forbiddenHatsImages[currentForbiddenHat].sprite = badHat.symbol;
            forbiddenHatsImages[currentForbiddenHat].color = Color.white;
            //modelsForHats[currentForbiddenHat].sprite = spritesModelsForHats[Random.Range(0, spritesModelsForHats.Length)];
            forbiddenHatsImages[currentForbiddenHat].gameObject.SetActive(true);
            //modelsForHats[currentForbiddenHat].gameObject.SetActive(true);
            if (!censoredSprites.Contains((badHat.symbol, badHat.uiSymbol)))
            {
                censoredSprites.Add((badHat.symbol, badHat.uiSymbol));
                newPolicemanCensoredSprites.Add(badHat.uiSymbol);
            }
            currentCensoredSprites.Add((badHat.symbol, badHat.uiSymbol));
            currentForbiddenHat++;
        }

        if (specialDay)
        {
            
            forbiddenHatsImages[currentForbiddenHat].sprite = data.specialHat.symbol;
            forbiddenHatsImages[currentForbiddenHat].color = Color.white;
            forbiddenHatsImages[currentForbiddenHat].gameObject.SetActive(true);
            //modelsForHats[currentForbiddenHat].gameObject.SetActive(true);
            if (!censoredSprites.Contains((data.specialHat.symbol, data.specialHat.uiSymbol)))
            {
                censoredSprites.Add((data.specialHat.symbol, data.specialHat.uiSymbol));
                newPolicemanCensoredSprites.Add(data.specialHat.uiSymbol);
            }
            currentCensoredSprites.Add((data.specialHat.symbol, data.specialHat.uiSymbol));
            currentForbiddenHat++;
        }

        for (int i = currentForbiddenHat; i < forbiddenHatsImages.Length; i++)
        {
            forbiddenHatsImages[i].gameObject.SetActive(false);
            //modelsForHats[i].gameObject.SetActive(false);
        }

        currentForbiddenHat = 0;
    }

    public (Sprite, bool) GetRandomHat(bool badHatToggle, bool special = false)
    {

        if(special && badHatToggle)
        {
            return (data.specialHat.symbol, true);
        }

        if (badHatToggle)
        {
            if (cloveSpawn)
            {
                return (data.specialHat.symbol, true);
            }

            TalkingData.TalkingTopics badHat;

            if (newBadHats.Count > 0)
            {
                int randomHat = Random.Range(0, newBadHats.Count);
                // Bad topic
                badHat = newBadHats[randomHat];
                newBadHats.Remove(badHat);
                return (badHat.symbol, true);
            }
            else
            {
                int randomHat = Random.Range(0, badHats.Count);
                // Bad Hat
                badHat = badHats[randomHat];
                return (badHat.symbol, true);
            }
        }
        else
        {

            int randomHat = Random.Range(0, goodHats.Count);
            // Good Hat
            TalkingData.TalkingTopics goodHat = goodHats[randomHat];
            return (goodHat.symbol, false);
        }

    }

    public void CreateNewTopics()
    {
        currentForbiddenWord = 0;
        goodTopics = new List<TalkingData.TalkingTopics>();
        tempGoodTopics = new List<TalkingData.TalkingTopics>();
        badTopics = new List<TalkingData.TalkingTopics>();

        //winningTeam.sprite = data.footballTeamWinning[winningFootBallTeam];

        if (badFootball)
        {
            if (!censoredSprites.Contains((data.footballTeamWinning[winningFootBallTeam], data.footballTeamWinning[winningFootBallTeam])))
            {
                censoredSprites.Add((data.footballTeamWinning[winningFootBallTeam], data.footballTeamWinning[winningFootBallTeam]));
                newPolicemanCensoredSprites.Add(data.footballTeamWinning[winningFootBallTeam]);
            }
            currentCensoredSprites.Add((data.footballTeamWinning[winningFootBallTeam], data.footballTeamWinning[winningFootBallTeam]));
        }

        goodTopics = data.topics.OfType<TalkingData.TalkingTopics>().ToList();
        tempGoodTopics = possibleBadTopics;

        for (int i = 0; i < numberOfBadTopics; i++)
        {
            //int randomBadTopic = Random.Range(0, goodTopics.Count);
            //TalkingData.TalkingTopics badTopic = goodTopics[randomBadTopic];
            TalkingData.TalkingTopics badTopic = tempGoodTopics[i];
            goodTopics.Remove(badTopic);
            badTopics.Add(badTopic);
            if (i >= numberOfBadTopics - newTopics)
            {
                newBadTopics.Add(badTopic);
            }
            forbiddenWordsImages[currentForbiddenWord].sprite = badTopic.symbol;
            forbiddenWordsImages[currentForbiddenWord].color = Color.white;
            forbiddenWordsImages[currentForbiddenWord].gameObject.SetActive(true);
            if (!censoredSprites.Contains((badTopic.symbol, badTopic.uiSymbol)))
            {
                censoredSprites.Add((badTopic.symbol, badTopic.uiSymbol));
                newPolicemanCensoredSprites.Add(badTopic.uiSymbol);
            }
            currentCensoredSprites.Add((badTopic.symbol, badTopic.uiSymbol));
            currentForbiddenWord++;
        }

        if (specialDay)
        {
            forbiddenWordsImages[currentForbiddenWord].sprite = data.specialTopic.symbol;
            forbiddenWordsImages[currentForbiddenWord].color = Color.white;
            forbiddenWordsImages[currentForbiddenWord].gameObject.SetActive(true);
            if (!censoredSprites.Contains((data.specialTopic.symbol, data.specialTopic.uiSymbol)))
            {
                censoredSprites.Add((data.specialTopic.symbol, data.specialTopic.uiSymbol));
                newPolicemanCensoredSprites.Add(data.specialTopic.uiSymbol);
            }
            currentCensoredSprites.Add((data.specialTopic.symbol, data.specialTopic.uiSymbol));
            currentForbiddenWord++;
        }

        List<TalkingData.TalkingTopics> tempList = data.alwaysBadTopics.OfType<TalkingData.TalkingTopics>().ToList();

        badTopics.Concat(tempList);

        for (int i = currentForbiddenWord; i < forbiddenWordsImages.Length; i++)
        {
            forbiddenWordsImages[i].gameObject.SetActive(false);
        }

        currentForbiddenWord = 0;
    }

    public (Sprite, bool) GetRandomTopic(bool badTopicToggle, bool special = false)
    {

        if(special && badTopicToggle)
        {
            return (data.specialTopic.symbol, true);
        }

        if (badTopicToggle)
        {
            if (cloveSpawn)
            {
                return (data.specialTopic.symbol, true);
            }

            int footballRNG = Random.Range(0, 100);

            if (footballRNG < chanceOfFootballTopic && badFootball)
            {
                int randomFootBallTeam = 0;
                do
                {
                    randomFootBallTeam = Random.Range(0, data.footballTeamWinning.Length);
                } while (winningFootBallTeam == randomFootBallTeam);

                return (data.footballTeamWinning[winningFootBallTeam], true);
            }
            else
            {
                TalkingData.TalkingTopics badTopic;

                if (newBadTopics.Count > 0)
                {
                    int randomTopic = Random.Range(0, newBadTopics.Count);
                    // Bad topic
                    badTopic = newBadTopics[randomTopic];
                    newBadTopics.Remove(badTopic);
                    return (badTopic.symbol, true);
                }
                else
                {
                    int randomTopic = Random.Range(0, badTopics.Count);
                    // Bad topic
                    badTopic = badTopics[randomTopic];
                    return (badTopic.symbol, true);
                }
            }
        }
        else
        {
            int footballRNG = Random.Range(0, 100);

            if (footballRNG < chanceOfFootballTopic && badFootball)
            {
                int randomFootBallTeam = 0;
                do
                {
                    randomFootBallTeam = Random.Range(0, data.footballTeamWinning.Length);
                } while (winningFootBallTeam == randomFootBallTeam);

                return (data.footballTeamWinning[randomFootBallTeam], false);
            }
            else
            {
                int randomTopic = Random.Range(0, goodTopics.Count);
                // Good Topic
                TalkingData.TalkingTopics goodTopic = goodTopics[randomTopic];
                return (goodTopic.symbol, false);
            }
        }
    }

    public void CreateSinging(bool badSinging)
    {
        //forbiddenSingingImage.gameObject.SetActive(badSinging);
        //forbiddenSingingImage.sprite = data.singing;
        this.badSinging = badSinging;

        if(badSinging)
        {
            if (!censoredSprites.Contains((data.singing, data.singing)))
            {
                censoredSprites.Add((data.singing, data.singing));
                newPolicemanCensoredSprites.Add(data.singing);
            }
            currentCensoredSprites.Add((data.singing, data.singing));
        }
    }

    public void CreateRadio(bool badRadio)
    {
        //forbiddenRadioImage.gameObject.SetActive(badRadio);
        //forbiddenRadioImage.sprite = data.radio;
        this.badRadio = badRadio;

        if (badRadio)
        {
            if (!censoredSprites.Contains((data.radio.symbol, data.radio.uiSymbol)))
            {
                censoredSprites.Add((data.radio.symbol, data.radio.uiSymbol));
                newPolicemanCensoredSprites.Add(data.radio.uiSymbol);
            }
            currentCensoredSprites.Add((data.radio.symbol, data.radio.uiSymbol));
        }
    }

    /// <summary>
    /// This method returns true to allow for NPCS to sing only if its not a day with censurable singing.
    /// </summary>
    /// <returns></returns>
    public bool GetRandomSinging()
    {
        if (!badSinging)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// This method returns true to allow for NPCS to have radios only if its not a day with censurable radio.
    /// </summary>
    /// <returns></returns>
    public bool GetRandomRadio()
    {
        if (!badRadio)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateCensorship(int currentLevel)
    {
        foreach ((Sprite, Sprite) sprite in censoredSprites) 
        {
            if (!currentCensoredSprites.Contains(sprite))
            {
                spritesToDelete.Add(sprite);
            }
        }

        foreach ((Sprite, Sprite) sprite in spritesToDelete)
        {
            censoredSprites.Remove(sprite);
        }

        if (currentLevel > imagesLevel || currentLevel == 0)
        {
            for (int i = 0; i < newPolicemanCensoredImages.Length; i++)
            {
                if (i < newPolicemanCensoredSprites.Count)
                {
                    newPolicemanCensoredImages[i].gameObject.SetActive(true);
                    newPolicemanCensoredImages[i].sprite = newPolicemanCensoredSprites[i];
                }
                else
                {
                    newPolicemanCensoredImages[i].gameObject.SetActive(false);
                }
            }

            imagesLevel = currentLevel;
        }

        for (int i = 0; i < allCensorImages.Length; i++)
        {
            if (i < censoredSprites.Count)
            {
                allCensorImages[i].gameObject.SetActive(true);
                allCensorImages[i].sprite = censoredSprites[i].Item2;
            }
            else
            {
                allCensorImages[i].gameObject.SetActive(false);
            }
        }
    }

    public void InitializeCensorshipItems()
    {
        winningFootBallTeam = Random.Range(0, data.footballTeamWinning.Length);

        possibleBadItems = new List<TalkingData.BookShapes>();
        possibleBadHats = new List<TalkingData.TalkingTopics>();

        List<TalkingData.BookShapes> booksToShuffle = new List<TalkingData.BookShapes>();
        List<TalkingData.BookShapes> itemsToShuffle = new List<TalkingData.BookShapes>();

        for (int i = 0; i < data.bookShapes.Length - 2; i++)
        {
            booksToShuffle.Add(data.bookShapes[i]);
        }

        for (int i = data.bookShapes.Length - 2; i < data.bookShapes.Length; i++)
        {
            itemsToShuffle.Add(data.bookShapes[i]);
        }

        booksToShuffle.Shuffle();
        itemsToShuffle.Shuffle();

        possibleBadItems.Add(booksToShuffle[0]);
        possibleBadItems.Add(booksToShuffle[1]);
        possibleBadItems.Add(booksToShuffle[2]);
        possibleBadItems.Add(itemsToShuffle[0]);
        possibleBadItems.Add(booksToShuffle[3]);
        possibleBadItems.Add(itemsToShuffle[1]);

        for (int i = booksToShuffle.Count - 4; i < booksToShuffle.Count; i++)
        {
            possibleBadItems.Add(booksToShuffle[i]);
        }


        List<TalkingData.TalkingTopics> initializerHatList = data.hats.OfType<TalkingData.TalkingTopics>().ToList();

        for (int i = 0; i < numberOfPossibleBadHats; i++)
        {
            possibleBadHats.Add(initializerHatList[i]);
        }

        possibleBadHats.Shuffle();

        List<TalkingData.TalkingTopics> initializerTopicList = new List<TalkingData.TalkingTopics>();
        List<TalkingData.TalkingTopics> initializedTopicList2 = data.topics.OfType<TalkingData.TalkingTopics>().ToList();

        for (int i = 0; i < 3; i++)
        {
            initializerTopicList.Add(initializedTopicList2[i]);
        }

        initializerTopicList.Shuffle();

        int randomOtherTopic = Random.Range(3, 5);

        if (randomOtherTopic == 3)
        {
            initializerTopicList.Add(initializedTopicList2[3]);
            initializerTopicList.Add(initializedTopicList2[4]);
        }
        else
        {
            initializerTopicList.Add(initializedTopicList2[4]);
            initializerTopicList.Add(initializedTopicList2[3]);
        }

        for (int i = 5; i < initializedTopicList2.Count; i++)
        {
            initializerTopicList.Add(initializedTopicList2[i]);
        }

        possibleBadTopics = initializerTopicList;
    }
}
