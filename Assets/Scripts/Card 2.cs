using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
//This creates a menu item called "Card"
public class Card2 : ScriptableObject
{
    public new string name;
    public string description;
    public int manaCost;
    public int attack;



    public void Print()
    {
        Debug.Log(name + ": " + description + " The card costs: " + manaCost);
    }




}
