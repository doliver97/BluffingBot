using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bet: MonoBehaviour {

    public Sprite[] sprites;

    private int bvalue;
    public int BetValue
    {
        get
        {
            return bvalue;
        }
        set
        {
            bvalue = value;
            SetSprite();
        }
    }

    // Use this for initialization
    void Start ()
    {
        BetValue = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetSprite()
    {
        string s = "chip" + BetValue;
        GetComponent<SpriteRenderer>().sprite = System.Array.Find(sprites, o => o.name == s);
    }
}
