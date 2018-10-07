using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Agent {

    //The higher card of the player
    public GameObject Hcard;
    //The lower card of the player
    public GameObject Lcard;

    //Enable if player is dealer
    public GameObject dealerButton;

    //Sprite of the bet placed in this round
    public GameObject Bet;

    //The player on its left
    public GameObject NextPlayer;

    //Account of the player
    private int chips;

    //Account text of the player
    public Text account;

    //Access to bet value
    public int BetValue
    {
        get
        {
            return Bet.GetComponent<Bet>().BetValue;
        }
    }

    //Bet to call in the decision
    private int betToCall; 

    //Folded its cards
    [HideInInspector]
    public bool folded;
    
    //Decision making is finished
    [HideInInspector]
    public bool isFinished;

    private float playerReward;

    // Use this for initialization
    void Start () {
        chips = 1000;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Init for new round
    public void NewRound()
    {
        Bet.GetComponent<Bet>().BetValue = 0;
        playerReward = 0;
    }

    //Calls the CollectObservation() and AgentAction() methods
    public void MakeDecision(int nbetToCall)
    {
        isFinished = false;

        betToCall = nbetToCall;

        folded = false; //init
        GetComponent<Agent>().RequestDecision(); //call when decision (and action) is needed
        
    }
    
    //Puts chips on the table
    public void PlaceBet(int b)
    {
        if (b > Bet.GetComponent<Bet>().BetValue)
        {
            Bet.GetComponent<Bet>().BetValue = b;
            //Debug.Log("BetValue set to " + b);
        }
    }

    public void WinChips(int i)
    {
        chips += i;
        account.text = chips.ToString();

        playerReward += i;
    }

    //Removing the bet in pot from the account
    public void LoseChips()
    {
        chips -= BetValue;
        //Debug.Log("Chips reduced by " + BetValue + " to " + chips);
        account.text = chips.ToString();

        playerReward -= BetValue;
    }

    //Sets the two cards by getting the strings representing the cards
    public void SetNewCards(string s1,string s2)
    {
        Hcard.GetComponent<Card>().StrName = s1;
        Lcard.GetComponent<Card>().StrName = s2;

        //switch if needed
        if (Hcard.GetComponent<Card>().Value < Lcard.GetComponent<Card>().Value)
        {
            Hcard.GetComponent<Card>().StrName = s2;
            Lcard.GetComponent<Card>().StrName = s1;

        }
    }

    /////////////////Agent specific functions///////////////////////////////////////////

    public override void CollectObservations()
    {
        //Debug.Log("Observations collected");

        //TODO we need cards, bet to call, opponents in before us, opponents behind us
        //Now only card values are implemented (current size: 2)

        //Hcard
        AddVectorObs(Hcard.GetComponent<Card>().Value);

        //Lcard
        AddVectorObs(Lcard.GetComponent<Card>().Value);
        
    }

    //act[] has 1 element between 0 and 3
    public override void AgentAction(float[] act, string textAction)
    {
        //Debug.Log("Bet to call:" + betToCall);


        if (act[0] == 0)
        {
            //Debug.Log("I fold");
            folded = true;
        }
        else if (act[0] == 1)
        {
            if (betToCall <= 1)
            {
                //Debug.Log("I bet 1");
                PlaceBet(1);
            }
            else
            {
                //Debug.Log("I fold");
                folded = true;
            }
        }
        else if (act[0] == 2)
        {
            if (betToCall <= 5)
            {
                //Debug.Log("I bet 5");
                PlaceBet(5);
            }
            else
            {
                //Debug.Log("I fold");
                folded = true;
            }
        }
        else if (act[0] == 3)
        {
            if (betToCall <= 25)
            {
                //Debug.Log("I bet 25");
                PlaceBet(25);
            }
            else
            {
                //Debug.Log("I fold");
                folded = true;
            }
        }

        AddReward(playerReward);

        isFinished = true;
        
    }

    
}
