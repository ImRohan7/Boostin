using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject levelHolder;
    public GameObject playerHolder;

    public int restartTime;

    public int playerCount;

    [Header("PREFAB REFERENCES")]
    public GameObject playerManager;

    private UnityEngine.Object[] levels;
    private GameObject currentLevel;

    private PlayerManager[] players;

    public UnityEngine.Object PlayLevel;

    public UnityEngine.Object[] playLevels;

    public PlayerManager[] playerManagers; // holds the player managers in Game

    // Level stuff
    public int RemainingPlayers;

    private BitchManager bm;

    private int numAlive;

    private float timer;

    


    private void Start()
    {
        numAlive = 0;
        timer = 0.0f;
        bm = GameObject.Find("Bitch Manager").GetComponent<BitchManager>();
        //print(bm.BWins);
        //print(bm.TWins);
        //print(bm.CWins);
        //print(bm.HWins);

        bm.showScoreonScreen();
        bm.spawnDeathShitWhennewRoundBegins();
       // UIHandler.Instance.showCountDown();

        if(bm.Firstgame)
        {
            bm.Firstgame = false;
            UIHandler.Instance.showCountDown();
            Debug.Log("FG");
        }
    }

    private void Awake()
    {
        Instance = this;

       
        levels = Resources.LoadAll("Levels", typeof(GameObject));
        playerManagers = new PlayerManager[playerCount];
        RemainingPlayers = 4;
        SetupNewLevel();
       
    }

    private void SetupNewLevel()
    {
        //spawn random level
        int rand = UnityEngine.Random.Range(0, playLevels.Length);
        currentLevel = Instantiate(playLevels[rand] as GameObject, Vector3.zero, Quaternion.identity, levelHolder.transform);

        //spawn players
        for(int i = 0; i < playerCount; i++)
        {
            GameObject newPlayer = Instantiate(playerManager, Vector3.zero, Quaternion.identity, playerHolder.transform);
            playerManagers[i] = newPlayer.GetComponent<PlayerManager>();
            playerManagers[i].InitializePlayerManager(currentLevel.GetComponent<LevelManager>().spawnPoints[i], i);
        }
    }


    // when player dies
    public void RegisterDeath()
    {
        RemainingPlayers--;

        if (checkForrestart())
        {
            UpdateScore();
            //RestarLevel();
        }
    }

    bool checkForrestart()
    {
        if (RemainingPlayers == 1)
            return true;
        else
            return false;
    }


    void RestarLevel()
    {
        RemainingPlayers = playerCount; // 4
       
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
     //   bm.showScoreonScreen();
    }


    void UpdateScore()
    {
        foreach(PlayerManager pm in playerManagers)
        {
            
        }
    }

    private void Update()
    {
        if (RemainingPlayers == 1)
        {
            StartCoroutine(EndRound());
        }
        timer += Time.deltaTime;
        if((int)timer > 30)
        {
            int x = UnityEngine.Random.Range(0, 1);
            if(x == 0)
            {
                GameObject.Find("LavaLeft").GetComponent<MoveLava>().right = true;
                GameObject.Find("LavaRight").GetComponent<MoveLava>().right = true;

            }
            else if(x == 1)
            {
                GameObject.Find("LavaLeft").GetComponent<MoveLava>().right = true;
                GameObject.Find("LavaRight").GetComponent<MoveLava>().right = true;

            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            bm.BWins = 0;
            bm.CWins = 0;
            bm.TWins = 0;
            bm.HWins = 0;
        }
    }

    IEnumerator EndRound()
    {
       RemainingPlayers = 0;
       foreach(PlayerManager p in playerManagers)
       {
            if(p.IsAlive)
            {
                if (p.playerID == 0)
                {
                    print("Bitch B Wins!");
                    bm.increaseBitchWin(ref bm.BWins);
                    //ScoreManager.Instance.showScore(p.playerID, bm.BWins);
                }
                else if(p.playerID == 1)
                {
                    print("Bitch T Wins!");
                    bm.increaseBitchWin(ref bm.TWins);
                   // ScoreManager.Instance.showScore(p.playerID, bm.TWins);
                }
                else if(p.playerID == 2)
                {
                    print("Bitch C Wins!");
                    bm.increaseBitchWin(ref bm.CWins);
                    //ScoreManager.Instance.showScore(p.playerID, bm.CWins);
                }
                else if (p.playerID == 3)
                {
                    print("Bitch H Wins!");
                    bm.increaseBitchWin(ref bm.HWins);
                   // ScoreManager.Instance.showScore(p.playerID, bm.HWins);
                }

                bm.showScoreonScreen();
                ScoreManager.Instance.spawnDeathIcons(p.playerID);

            }
       }
        if(!bm.CheckForWin())
        {

            UIHandler.Instance.showCountDown();
            yield return new WaitForSeconds(restartTime);
            //TO DO: Add restarting and keeping track of rounds
            RestarLevel();
            yield return null;
        }
        else
        {
            DisplayWinner();
        }
    }

    private void DisplayWinner()
    {
        bm.clearIcons();
        bm.spawnDeathShitWhennewRoundBegins();
        print("Game Over.");
        int maxVal = bm.bitchArray.Max();
        int maxIndex = bm.bitchArray.ToList().IndexOf(maxVal);
        if(maxIndex == 0)
        {
            print("Bitch B Wins the Game!");
        }
        else if(maxIndex == 1)
        {
            print("Bitch T Wins the Game!");
        }
        else if(maxIndex == 2)
        {
            print("Bitch C Wins the Game!");
        }
        else if(maxIndex == 3)
        {
            print("Bitch H Wins the Game!");
        }
    }
}
