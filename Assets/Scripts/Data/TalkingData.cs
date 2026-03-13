using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TalkingData", menuName = "Scriptable Objects/TalkingData")]
public class TalkingData : ScriptableObject
{


    public enum TypeOfConversation
    {
        Football,
        RandomTopics,
        Hat,
        Book

    }

    public enum BookShape
    {
        Triangle,
        Ball,
        Square

    }




    [Serializable]
    public struct TalkingTopics
    {
        public Sprite symbol;
        public TypeOfConversation type;
        public Sprite uiSymbol;
    }

    [Serializable]
    public struct BookShapes
    {
        public Sprite book;
        public Sprite uiBook;
    }

    [Serializable]
    public struct Radio
    {
        public Sprite symbol;
        public Sprite uiSymbol;
    }

    [Serializable]
    public struct SpecialItem
    {
        public Sprite symbol;
        public Sprite uiSymbol;
    }

    [Serializable]
    public struct SpecialHat
    {
        public Sprite symbol;
        public Sprite uiSymbol;
    }

    [Serializable]
    public struct SpecialTopic
    {
        public Sprite symbol;
        public Sprite uiSymbol;
    }



    public Color[] bookColors;
    public BookShapes[] bookShapes;

    public Sprite[] footballTeamWinning;
    public Sprite[] footballTeamLogo;
    public TalkingTopics[] topics;
    public TalkingTopics[] alwaysBadTopics;
    public TalkingTopics[] hats;

    public Sprite singing;
    public Radio radio;
    public SpecialItem specialItem;
    public SpecialHat specialHat;
    public SpecialTopic specialTopic;
}
