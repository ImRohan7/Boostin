﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BitchManager : MonoBehaviour
{
    [Header("Number of times each bitch has won")]
    public int BWins;
    public int TWins;
    public int CWins;
    public int HWins;

    public int numRounds;

    public int[] bitchArray;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
     
        BWins = 0;
        TWins = 0;
        CWins = 0;
        HWins = 0;
        numRounds = 0;

        bitchArray = new int[] { BWins, TWins, CWins, HWins };

       
    }

    public int[] increaseBitchWin(ref int bitchWhichWon)
    {
        bitchWhichWon++;
        numRounds++;
        bitchArray = new int[] { BWins, TWins, CWins, HWins };

        return bitchArray;
    }

    public bool CheckForWin()
    {
        int maxVal = bitchArray.Max();
        int maxIndex = bitchArray.ToList().IndexOf(maxVal);

        if(maxVal == 3)
        {
            return true;
        }
        else if(numRounds == 5)
        {
            return true;
        }
        return false;
    }

    public void showScoreonScreen()
    {
      //  ScoreManager.Instance.initVars();
        ScoreManager.Instance.showScore(0, BWins);
        ScoreManager.Instance.showScore(1, TWins);
        ScoreManager.Instance.showScore(2, CWins);
        ScoreManager.Instance.showScore(3, HWins);
    }

    public void clearIcons()
    {
        for(int i=0;i<4;i++)
        {
            ScoreManager.Instance.clear(i);
        }
    }

    public void spawnDeathShitWhennewRoundBegins()
    {
        for(int i=0; i<BWins; i++)
        {
            ScoreManager.Instance.spawnDeathIcons(0);
        }

        for (int i = 0; i < TWins; i++)
        {
            ScoreManager.Instance.spawnDeathIcons(1);
        }

        for (int i = 0; i < CWins; i++)
        {
            ScoreManager.Instance.spawnDeathIcons(2);
        }

        for (int i = 0; i < HWins; i++)
        {
            ScoreManager.Instance.spawnDeathIcons(3);
        }
    }
}
