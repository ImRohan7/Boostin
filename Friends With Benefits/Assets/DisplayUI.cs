using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* This script is for
 * Displaying score 
 * Displaying twitch stuff
 */ 

public class DisplayUI : MonoBehaviour
{

    public int ID; // player id
    public TMP_Text txtScore;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // update score text
    public void updateScore(int iScore)
    {
        txtScore.text = iScore.ToString();
    }

    // 
    public void show_countdown_Timer()
    {

    }
    
}
