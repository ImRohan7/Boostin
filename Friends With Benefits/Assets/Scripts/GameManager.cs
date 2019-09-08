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
    private void Awake()
    {
        levels = Resources.LoadAll("Levels", typeof(GameObject));
        SetupNewLevel();
    }

    private void Update()
    {
    }
    private void SetupNewLevel()
    {
        //spawn random level
        int rand = Random.Range(0, levels.Length);
        currentLevel = Instantiate(levels[rand] as GameObject, Vector3.zero, Quaternion.identity, levelHolder.transform);

        //spawn players
        for(int i = 0; i < playerCount; i++)
        {
            GameObject newPlayer = Instantiate(playerManager, Vector3.zero, Quaternion.identity, playerHolder.transform);
            newPlayer.GetComponent<PlayerManager>().InitializePlayerManager(currentLevel.GetComponent<LevelManager>().spawnPoints[i], i);
        }
    }
}
