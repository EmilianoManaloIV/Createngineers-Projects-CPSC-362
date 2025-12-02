using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //You will need this for the "Text" variables
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour
{
    public Card2 card;

    public Text nameText;
    public Text descriptionText;
    public Text manaText;
    public Text attackText;


    // Start is called before the first frame update
    void Start()
    {
        nameText.text = card.name;
        descriptionText.text = card.description;
        manaText.text = card.manaCost.ToString();
        attackText.text = card.attack.ToString();
    }
}
