using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject levelHolder;
    public GameObject playerHolder;

    public int playerCount;

    [Header("PREFAB REFERENCES")]
    public GameObject playerManager;

    private Object[] levels;
    private GameObject currentLevel;

    private PlayerManager[] players;

    public Object PlayLevel;

    private void Awake()
    {
        levels = Resources.LoadAll("Levels", typeof(GameObject));
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
            newPlayer.GetComponent<PlayerManager>().InitializePlayerManager(currentLevel.GetComponent<LevelManager>().spawnPoints[i], i);
        }
    }

    private void Update()
    {
        players = Object.FindObjectsOfType<PlayerManager>();

        if(players.Length == 1)
        {
            StartCoroutine(EndRound(players[0].playerID));
            print(players[0].playerID);
        }
    }

    IEnumerator EndRound(int playerID)
    {
        if (playerID == 0)
        {

        }
        else if (playerID == 1)
        {

        }
        else if (playerID == 2)
        {

        }
        else if (playerID == 3)
        {

        }

        yield return new WaitForSeconds(3f);

        //TO DO: Add restarting and keeping track of rounds
    }
}
