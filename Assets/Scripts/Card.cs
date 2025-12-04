using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Card
{
    public int id;
    public int value;
    public string color;
    //public Sprite spriteImage;



    public Card()
    {

    }



    public Card(int Id, int Value, string Color)
    {
        id = Id;
        value = Value;
        color = Color;
        //spriteImage = SpriteImage;
    }




}
