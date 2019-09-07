using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerCharacterPrefab;
    private GameObject currentPlayerCharacter;
    
    private Transform spawnPoint;
    private int playerID;

    [SerializeField]
    private int killCount;

    public void InitializePlayerManager(Transform newSpawnPoint, int newPlayerID)
    {
        spawnPoint = newSpawnPoint;
        playerID = newPlayerID;

        PlayerSpawn();
    }

    private void PlayerSpawn()
    {
        currentPlayerCharacter = Instantiate(playerCharacterPrefab, spawnPoint.position, Quaternion.identity, transform);

        currentPlayerCharacter.GetComponent<PlayerController>().InitializePlayerController(this, playerID);
    }

    public void PlayerDeath()
    {
        if (currentPlayerCharacter != null)
        {
            Destroy(currentPlayerCharacter);
            currentPlayerCharacter = null;
            StartCoroutine(PlayerRespawn());

            CameraController.instance.TriggerShake(0.2f, 0.25f);
        }
    }

    public void PlayerKill()
    {
        killCount++;
        ScoreManager.Instance.increaseScore(playerID, killCount);
    }

    private IEnumerator PlayerRespawn()
    {
        yield return new WaitForSeconds(3);

        PlayerSpawn();
    }
}
