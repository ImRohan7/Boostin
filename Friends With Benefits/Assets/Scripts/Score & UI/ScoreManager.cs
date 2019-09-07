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
    private DisplayStats[] displayStats;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        displayStats = new DisplayStats[players];
        displayStats = parent_ScoreWidgets.transform.GetComponentsInChildren<DisplayStats>();

    }

    public void increaseScore(int id, int killCpunt)
    {
        displayStats[id].updateScore(killCpunt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
