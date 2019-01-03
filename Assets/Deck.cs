using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents the deck on the top of the table
public class Deck : MonoBehaviour {

    private static System.Random rng = new System.Random();

    private List<string> cards = new List<string>()
        {
            "2c",
            "2d",
            "2h",
            "2s",
            "3c",
            "3d",
            "3h",
            "3s",
            "4c",
            "4d",
            "4h",
            "4s",
            "5c",
            "5d",
            "5h",
            "5s",
            "6c",
            "6d",
            "6h",
            "6s",
            "7c",
            "7d",
            "7h",
            "7s",
            "8c",
            "8d",
            "8h",
            "8s",
            "9c",
            "9d",
            "9h",
            "9s",
            "Tc",
            "Td",
            "Th",
            "Ts",
            "Jc",
            "Jd",
            "Jh",
            "Js",
            "Qc",
            "Qd",
            "Qh",
            "Qs",
            "Kc",
            "Kd",
            "Kh",
            "Ks",
            "Ac",
            "Ad",
            "Ah",
            "As"
        };

    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //Shuffles the deck
    public void Shuffle()
    {
        int n = cards.Count; //52
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            string value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }

    //Returns with n random cards
    public List<string> GiveNCards(int n)
    {
        Shuffle();
        Shuffle();
        Shuffle();
        return cards.GetRange(0, n);
    }
}
