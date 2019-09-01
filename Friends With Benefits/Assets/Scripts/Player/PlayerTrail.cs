using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    public ParticleSystem trailParticleSystem;

    private PlayerController player;
    private List<ParticleCollisionEvent> collisionEvents;

    private void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    public void AttachToPlayer(PlayerController pc)
    {
        player = pc;
        ParticleSystem.CollisionModule collisionModule = trailParticleSystem.collision;

        LayerMask layer0 = 1 << LayerMask.NameToLayer("Player0");
        LayerMask layer1 = 1 << LayerMask.NameToLayer("Player1");
        LayerMask layer2 = 1 << LayerMask.NameToLayer("Player2");
        LayerMask layer3 = 1 << LayerMask.NameToLayer("Player3");

        switch (player.GetID())
        {
            case 0:
                collisionModule.collidesWith = layer1 | layer2 | layer3;
                break;
            case 1:
                collisionModule.collidesWith = layer0 | layer2 | layer3;
                break;
            case 2:
                collisionModule.collidesWith = layer0 | layer1 | layer3;
                break;
            case 3:
                collisionModule.collidesWith = layer0 | layer1 | layer2;
                break;
            default:
                break;
        }
    }

    void OnParticleCollision(GameObject other)
    {
        int collisionCount = trailParticleSystem.GetCollisionEvents(other, collisionEvents);

        for(int i = 0; i < collisionCount; i++)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                if(other.gameObject != player.gameObject)
                {
                    //currently just destroying other player, but could be extended to add bounce or anything else
                    other.gameObject.GetComponent<PlayerController>().TriggerDeath();
                    player.TriggerKill();
                    Destroy(other);
                }
            }
        }
    }
}
