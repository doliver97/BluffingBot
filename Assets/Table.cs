using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour {

    //Set in the editor
    public List<GameObject> Players;

    public GameObject deck;

    //Increased each round
    private int dealerIndex;

    //The maximum bet in this round
    public int maxBetValue;

    //Round counter
    public Text roundText;
    private int round;

    //Speed of the simulation
    public Text simSpeed;
    private int speed;
    private bool canMeasureSpeed;
    private int prevRoundNumber;

    //Pot
    private int pot;

    //Players who havent folded
    private List<GameObject> PlayersIn;

    //Cant evaluate til betting is running
    private bool finishedBetting;

    private bool isRoundOver;
    private bool isPlayOneRoundRunning;

	void Start ()
    {
        round = 0;
        isRoundOver = true;
        canMeasureSpeed = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isRoundOver)
        {
            StartCoroutine(Deal());
        }

        if(canMeasureSpeed)
        {
            StartCoroutine(MeasureSpeed());
        }
	}

    //Deals random cards to everyone, and plays a full round of game
    public IEnumerator Deal()
    {
        isRoundOver = false;
        
        List<string> dealtCards = deck.GetComponent<Deck>().GiveNCards(Players.Count * 2);

        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].GetComponent<Player>().SetNewCards(dealtCards[i * 2], dealtCards[i * 2 + 1]);

            //Setting the dealer button
            if (dealerIndex % Players.Count == i)
            {
                Players[i].GetComponent<Player>().dealerButton.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                Players[i].GetComponent<Player>().dealerButton.GetComponent<Renderer>().enabled = false;
            }
        }


        StartCoroutine(PlayOneRound());

        while(isPlayOneRoundRunning)
        {
            yield return null;
        }

        round++;
        roundText.text = "Round: " + round ;

        isRoundOver = true;
    }

    //Representing 1 round of the game
    public IEnumerator PlayOneRound()
    {
        isPlayOneRoundRunning = true;
        //Debug.Log("PlayOneRound started");

        //Filling PlayersIn
        PlayersIn = new List<GameObject>();
        for (int i = 0; i < Players.Count; i++)
        {
            PlayersIn.Add(Players[i]);
        }

        //Init players for new round
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].GetComponent<Player>().NewRound();
        }

        maxBetValue = 1;
        pot = 0;


        finishedBetting = false;

        //Betting
        StartCoroutine(Betting());

        while(!finishedBetting)
        {
            yield return null;
        }

        //Evaluating result
        Evaluating();

        //Moving the dealer button
        dealerIndex++;

        isPlayOneRoundRunning = false;
        //Debug.Log("PlayOneRound finished");
    }

    

    //Goes around the players, asking what they do
    public IEnumerator Betting()
    {

        GameObject actualPlayer = Players[(dealerIndex+1) % Players.Count]; //The first player after dealer starts the betting

        int stuck = 0; //[DEBUG]

        //Debug.Log(actualPlayer.name + " starts the round");

        while (actualPlayer.GetComponent<Player>().BetValue < maxBetValue)
        {
            if (stuck++ > 100)
            {
                Debug.LogError("Stuck in a while loop in Betting()");
                yield return null; //Debug, not really a yield
            }


            //Debug.Log("Decision making started",actualPlayer);

            //HERE plays the player
            actualPlayer.GetComponent<Player>().MakeDecision(maxBetValue);

            //Waiting for Agent to make a decision
            while(!actualPlayer.GetComponent<Player>().isFinished)
            {
                yield return null;
            }


            //Debug.Log("Decision making has ended",actualPlayer);

            //do not fold if only player in pot
            if (actualPlayer.GetComponent<Player>().folded && PlayersIn.Count>1)
            {
                PlayersIn.Remove(actualPlayer);
            }

            //Setting the needed bet
            //Debug.Log("Read player betvalue: " + actualPlayer.GetComponent<Player>().BetValue);
            maxBetValue = Mathf.Max(maxBetValue,actualPlayer.GetComponent<Player>().BetValue);


            //Go to next player
            actualPlayer = actualPlayer.GetComponent<Player>().NextPlayer;
            while (PlayersIn.Count>0 && !PlayersIn.Contains(actualPlayer))
            {
                actualPlayer = actualPlayer.GetComponent<Player>().NextPlayer;
                
            }
        }

        finishedBetting = true;
    }

    //Evaluating the results, giving the pot to the winner
    public void Evaluating()
    {

        //Delete rewards before setting
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].GetComponent<Player>().playerReward = 0;
        }

        if (PlayersIn.Count==0)
        {
            Debug.Log("Error: no players in pot");
            return;
        }


        GameObject bestPlayer = PlayersIn[0];
        int bestHvalue = bestPlayer.GetComponent<Player>().Hcard.GetComponent<Card>().Value;
        int bestLvalue = bestPlayer.GetComponent<Player>().Lcard.GetComponent<Card>().Value;

        for (int i = 1; i < PlayersIn.Count; i++)
        {
            int Hvalue = PlayersIn[i].GetComponent<Player>().Hcard.GetComponent<Card>().Value;
            int Lvalue = PlayersIn[i].GetComponent<Player>().Lcard.GetComponent<Card>().Value;

            if (Hvalue > bestHvalue)
            {
                bestPlayer = PlayersIn[i]; 
            }
            else if (Hvalue == bestHvalue && Lvalue > bestLvalue)
            {
                bestPlayer = PlayersIn[i];
            }
        }

        //Everyone is losing their chips in pot (including the winner, and players who folded)
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].GetComponent<Player>().LoseChips();
            pot += Players[i].GetComponent<Player>().BetValue;
        }

        //Giving the pot to the winner
        bestPlayer.GetComponent<Player>().WinChips(pot);
        pot = 0;
    }

    private IEnumerator MeasureSpeed()
    {
        canMeasureSpeed = false;
        
        int nRounds = round - prevRoundNumber;
        
        simSpeed.text = "Speed: " + nRounds +" round/s";

        prevRoundNumber = round;

        yield return new WaitForSeconds(1);

        canMeasureSpeed = true;
    }
}
