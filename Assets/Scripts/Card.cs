using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]// Information on this will be in part 2 or 3

public class Card
{ 
    public int id; //identifcation for card
    public string cardName;
    public int cost;
    public int power;
    public string cardDescription;

    public Card()
    {

    }

    public Card(int Id, string CardName, int Cost, int Power,string CardDescription)
    {
        id = Id;
        cardName = CardName;
        cost = Cost;
        power = Power;
        cardDescription = CardDescription;
    }
}
