using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject levelHolder;
    public GameObject playerHolder;

    public int playerCount;

    [Header("PREFAB REFERENCES")]
    public GameObject playerManager;

    private Object[] levels;
    private GameObject currentLevel;

    public Object PlayLevel;

    public PlayerManager[] playerManagers; // holds the player managers in Game

    // Level stuff
    public int RemainingPlayers;


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
        //int rand = Random.Range(0, levels.Length);
        currentLevel = Instantiate(PlayLevel as GameObject, Vector3.zero, Quaternion.identity, levelHolder.transform);

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
            RestarLevel();
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

        foreach (PlayerManager pm in playerManagers)
        {
            pm.PlayerSpawn();
            
        }
    }


    void UpdateScore()
    {
        foreach(PlayerManager pm in playerManagers)
        {
            if(pm.IsAlive)
            {
                pm.score++;
                ScoreManager.Instance.showScore(pm.playerID, pm.score);
            }
        }
    }
}
