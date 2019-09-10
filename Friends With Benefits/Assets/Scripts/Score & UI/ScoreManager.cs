using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This will
 * Keep a track of score and tells DisplayStats to update UI
 * 
 */
 
public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance;

    [Header("Number of players")]
    public int players;

    public GameObject parent_ScoreWidgets;

    [SerializeField]
    private DisplayUI[] displayStats;

    public int counter = 0;

    private void Awake()
    {
        Instance = this;
        initVars();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void clear(int id)
    {
        displayStats[id].clear();
    }

    public void spawnDeathIcons(int id)
    {
        displayStats[id].spawnDeathIcon();
    }

    public void showScore(int id, int score)
    {
        displayStats[id].updateScore(score);
    }

   public void initVars()
   {
        parent_ScoreWidgets = GameObject.Find("Score Widgets");
        displayStats = new DisplayUI[players];
        displayStats = parent_ScoreWidgets.transform.GetComponentsInChildren<DisplayUI>();
   }

    public void showMVB(int id)
    {
        for (int i = 0; i < displayStats.Length; i++)
        {
            if (i == id)
            {
                displayStats[i].show_MVBLogo();
            }
            else
            {
                displayStats[i].show_originalLogo();
            }
        }
    }
}
