using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    private string strName;

    //string representation of the card, sets the sprite automatically
    public string StrName
    {
        get
        {
            return strName;
        }
        set
        {
            strName = value;
            SetSprite();
        }
    }
    
    public Sprite[] sprites;

    //A char representing the face value of the card (eg. A,T,3)
    public char FaceValue
    {
        get
        {
            return StrName[0];
        }
    }

    //Helps comparing cards
    public int Value
    {
        get
        {
            switch(FaceValue)
            {
                case '2': return 2;
                case '3': return 3;
                case '4': return 4;
                case '5': return 5;
                case '6': return 6;
                case '7': return 7;
                case '8': return 8;
                case '9': return 9;
                case 'T': return 10;
                case 'J': return 11;
                case 'Q': return 12;
                case 'K': return 13;
                case 'A': return 14;
                default: return 0;
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        StrName = "backside";
    }

    // Update is called once per frame
    void Update ()
    {

    }

    void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = System.Array.Find(sprites, o => o.name == StrName);
    }
}
