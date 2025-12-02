using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //You will need this for the "Text" variables
using UnityEngine.EventSystems;

public class DisplayCard : MonoBehaviour
{
    public List<Card> displayCard = new List<Card>();

    public int id;
    public int value;
    public string color;

    public Text ValueText;
    public Image CardColor;

    public bool cardBack;
    public static bool staticCardBack;

    public GameObject Hand;
    public int numberOfCardsInDeck;
  

    // Start is called before the first frame update
    void Start()
    {
        //numberOfCardsInDeck = PlayerDeck.deckSize;

        //To change the values in the FIRST card in the selection
        displayCard[0] = CardDatabase.cardList[id];

    }

    // Update is called once per frame
    void Update()
    {
        if (displayCard.Count == 0 || displayCard[0] == null) return;
        if (ValueText == null) return;


        id = displayCard[0].id;
        value = displayCard[0].value;
        color = displayCard[0].color;

        if (value == 10)
        {
            ValueText.text = " " + "Skip";
            ValueText.fontSize = 45;
        }
        else if (value == 11)
        {
            ValueText.text = " " + "Reverse";
            ValueText.fontSize = 30;
        }
        else if (value == 12)
        {
            ValueText.text = " " + "Draw 2";
            ValueText.fontSize = 40;
        }
        else if (value == 13)
        {
            ValueText.text = " " + "WILD";
            ValueText.fontSize = 45;
        }
        else if (value == 14)
        {
            ValueText.text = " " + "WILD Draw 4";
            ValueText.fontSize = 28;
        }
        else
        {
          ValueText.text = " " + value;
          ValueText.fontSize = 90;
        }

        if (color == "None")
        {
            CardColor.color = Color.gray;
            ValueText.color = Color.black;
        }
        else if (color == "Red")
        {
            CardColor.color = Color.red;
            ValueText.color = Color.black;
        }
        else if (color == "Yellow")
        {
            CardColor.color = Color.yellow;
            ValueText.color = Color.black;
        }
        else if (color == "Green")
        {
            CardColor.color = Color.green;
            ValueText.color = Color.black;
        }
        else if (color == "Blue")
        {
            CardColor.color = Color.blue;
            ValueText.color = Color.black;
        }
        else if (color == "Wild")
        {
            CardColor.color = Color.black;
            ValueText.color = Color.white;
        }



    /*    Hand = GameObject.Find("Hand");
        if(this.transform.parent == Hand.transform.parent)
        {
            cardBack = false;
        }
        staticCardBack = cardBack;

        if (this.tag == "Clone")
        {
            displayCard[0] = PlayerDeck.staticDeck[numberOfCardsInDeck - 1];
            numberOfCardsInDeck -= 1;
            PlayerDeck.deckSize -= 1;
            cardBack = false;
            this.tag = "Untagged";
        }
     */
    }
}
