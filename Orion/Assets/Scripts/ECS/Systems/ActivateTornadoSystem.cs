using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

using Unity.Jobs;
using Unity.Entities;
using Unity.Collections;
using Unity.Rendering;
using UnityEngine;
using Unity.Burst;

public class ActivateTornadoSystem : JobComponentSystem
{
    private float spawnTimer = 0f;
    private Unity.Mathematics.Random random;
    private float3 position;


    protected override void OnCreate()
    {
        random = new Unity.Mathematics.Random(56);
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        float deltatime = Time.DeltaTime;

        bool drapcastSpell2 = false;
        EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

        Entities.ForEach((Entity e, ref Translation translation, ref BossStats bossStats, ref Spell2Available castSpell2) =>
        {
            drapcastSpell2 = true;
            commandBuffer.RemoveComponent<Spell2Available>(e);

            bossStats.timeLeftSpell2 = bossStats.cdSpell2;
            WaterBossBehaviourTree._instance.timeLeftSpell2 = bossStats.timeLeftSpell2;

        }).Run();

        if (drapcastSpell2)
        {



            position = new float3(random.NextFloat(-50f, 50f), 0f, random.NextFloat(-50f, 50f));

            spawnTimer = 0;

            bool drap = true;
            for (int i = 0; i < 500; i = i + 1)
            {

                //On accède à notre entity grace à la variable globale du PrefabEntityComponent
                Entity spawnedEntity = EntityManager.Instantiate(PrefabEntityComponent.waterDropPrefabEntity);

                commandBuffer.AddComponent(spawnedEntity, new TornadoTag());

                EntityManager.SetComponentData(spawnedEntity, new Translation { Value = new float3(position.x, position.y + (0.10f * i), position.z) });

                commandBuffer.AddComponent(spawnedEntity, new WaterDropTag());

                if (drap)
                {
                    commandBuffer.AddComponent(spawnedEntity, new TornadoMovementData { duration = 15, initialPos = position, dropRotation = 1, timeCounter = 0f, speed = random.NextFloat(1, 5), width = random.NextFloat(0.6f, 1f) * (1f + (0.05f * i)), height = 7f });

                }
                else
                {
                    commandBuffer.AddComponent(spawnedEntity, new TornadoMovementData { duration = 15, initialPos = position, dropRotation = -1, timeCounter = 0f, speed = random.NextFloat(1, 5), width = random.NextFloat(0.6f, 1f) * (1f + (0.05f * i)), height = 7f });

                }

                drap = !drap; 

            }



        }

        Entities.ForEach((Entity e, ref Translation translation, ref BossStats bossStats, ref Spell2Available castSpell2) =>
        {

            if (bossStats.timeLeftSpell2 >= 0)
            {
                bossStats.timeLeftSpell2 = bossStats.timeLeftSpell2 - deltatime;
                WaterBossBehaviourTree._instance.timeLeftSpell2 = bossStats.timeLeftSpell2;
            }


        }).Run();

        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();

        return default;
    }

}
