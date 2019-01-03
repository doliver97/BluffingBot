using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyHeuristicDecision : MonoBehaviour, Decision
{
    static System.Random r = new System.Random();

    public static System.Random R { get => r; set => r = value; }

    public float[] Decide(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        //0: Hcard
        //1: Lcard

        int bet = 0;

        if (vectorObs[0] >= 13) // K or higher
        {
            bet = 3;
        }
        else if (vectorObs[0] >= 10) // 10 or higher
        {
            bet = 2;
        }
        else
        {
            bet = 1;
        }

        //int bet = R.Next(4);

        return new float[1] { bet };
    }

    public List<float> MakeMemory(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        //Dont need it now
        return new List<float>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
