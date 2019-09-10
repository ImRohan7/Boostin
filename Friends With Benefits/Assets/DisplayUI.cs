using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/* This script is for
 * Displaying score 
 * Displaying twitch stuff
 */ 
    
public class DisplayUI : MonoBehaviour
{

    public int ID; // player id
    public TMP_Text txtScore;

    public Sprite img_logo_original;
    public Image img_logo;
    public Image deathIcon;

    public GameObject objParentDeathIcons;

    // Start is called before the first frame update
    void Start()
    {
        img_logo_original = img_logo.sprite;
       // spawnDeathIcon();
    }

    // update score text
    public void updateScore(int iScore)
    {
        txtScore.text = iScore.ToString();
    }

    // 
    public void show_MVBLogo()
    {
        img_logo.sprite = UIHandler.Instance.sprite_MVB;
    }
    
    public void show_originalLogo()
    {
        img_logo.sprite = img_logo_original;
    }

    public void spawnDeathIcon()
    {
        Instantiate(deathIcon, objParentDeathIcons.transform);
    }

    public void clear()
    {
        for(int i=0; i<objParentDeathIcons.transform.childCount;i++)
        {
            Destroy(objParentDeathIcons.transform.GetChild(i).gameObject);

        }
    }
}
