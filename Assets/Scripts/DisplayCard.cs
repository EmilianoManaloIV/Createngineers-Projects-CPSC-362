using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayCard : MonoBehaviour
{
    public List<Card> displayCard = new List<Card>();
    //List must be in all lowercase to not confuse with name of class
    public int displayId;

    public int id; //identifcation for card
    public string cardName;
    public int cost;
    public int power;
    public string cardDescription;

    public Text nameText;
    public Text costText;
    public Text powerText;
    public Text descriptionText;
    
    // Start is called before the first frame update
    void Start()
    {
        displayCard[0] = CardDatabase.cardList[displayId];
    }

    // Update is called once per frame
    void Update()
    {
        id = displayCard[0].id;
        cardName = displayCard[0].cardName;
        cost = displayCard[0].cost;
        power = displayCard[0].power;
        cardDescription = displayCard[0].cardDescription;

        nameText.text = " " + cardName;
        costText.text = " " + cost;
        powerText.text = " " + power;
        descriptionText.text = " " + cardDescription;
    }
}
