using UnityEngine;

public enum CardColor { Red, Yellow, Green, Blue, Wild }
public enum CardType { Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Skip, Reverse, DrawTwo, Wild, WildDrawFour }

// This uses a ScriptableObject to store the data for each card.
[CreateAssetMenu(fileName = "New Card", menuName = "UNO/Card Data")]
public class CardData : ScriptableObject
{
    [Header("Card Properties")]
    public CardColor cardColor;
    public CardType cardType;

    [Header("Visuals")]
    public Sprite cardImage; // The sprite for the card's face (Red 5, Blue Skip, etc.)
    
    // Number cards are their face value, special cards are 20, wilds are 50.
    public int Value
    {
        get
        {
            switch (cardType)
            {
                case CardType.Zero: return 0;
                case CardType.One: return 1;
                case CardType.Two: return 2;
                case CardType.Three: return 3;
                case CardType.Four: return 4;
                case CardType.Five: return 5;
                case CardType.Six: return 6;
                case CardType.Seven: return 7;
                case CardType.Eight: return 8;
                case CardType.Nine: return 9;
                case CardType.Skip: return 20;
                case CardType.Reverse: return 20;
                case CardType.DrawTwo: return 20;
                case CardType.Wild: return 50;
                case CardType.WildDrawFour: return 50;
                default: return 0;
            }
        }
    }
}