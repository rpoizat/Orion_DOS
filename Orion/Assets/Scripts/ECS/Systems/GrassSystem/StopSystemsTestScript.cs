using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class StopSystemsTestScript : MonoBehaviour
{
    private EntityManager eManager;

    // Start is called before the first frame update
    void Start()
    {
        eManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        eManager.World.GetExistingSystem<PlayerMovementSystem>().Enabled = false;
        eManager.World.GetExistingSystem<GrenadeMovementSystem>().Enabled = false;
        eManager.World.GetExistingSystem<ActivateGlobalAttackSystem>().Enabled = false;
        eManager.World.GetExistingSystem<TriggerSystem>().Enabled = false;
        eManager.World.GetExistingSystem<HitBossCollisionSystem>().Enabled = false;
        eManager.World.GetExistingSystem<ExplosionSystem>().Enabled = false;
    }
}
