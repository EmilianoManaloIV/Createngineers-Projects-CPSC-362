using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //For Image and Text variables

public class OppHp : MonoBehaviour
{
    public static float maxHp;
    public static float staticHp;
    public float hp;
    public Image Health;
    public Text hpText;

    // Start is called before the first frame update
    void Start()
    {
        maxHp = 25000;
        staticHp = 5000;
    }

    // Update is called once per frame
    void Update()
    {
        hp = staticHp;
        Health.fillAmount = hp / maxHp;

        if (hp >= maxHp)
        {
            hp = maxHp;
        }

        hpText.text = hp + "HP";
    }
}
