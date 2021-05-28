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

[AlwaysSynchronizeSystem]

public class ActivateWaterNovaSystem : JobComponentSystem
{
    private float spawnTimer = 0f;
    private Unity.Mathematics.Random random;
    private float3 position;
    private bool drap;

    protected override void OnCreate()
    {
        random = new Unity.Mathematics.Random(56);

         drap = true;
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

        Vector3 bossPosition = new Vector3();

        Entities.ForEach((Entity e, ref Translation translation, ref Rotation rotation, ref BossStats bossStats) =>
        {
            bossPosition = translation.Value;
            
        }).Run();



        float radius = 3f;


        if (drap)
        {

            for (int i = 0; i < 8; i++)
            {

                //On accède à notre entity grace à la variable globale du PrefabEntityComponent
                Entity spawnedEntity = EntityManager.Instantiate(PrefabEntityComponent.waterDropPrefabEntity);

                float angle = i * Mathf.PI * 2f / 8;
                Vector3 newPos = new Vector3(bossPosition.x + Mathf.Cos(angle) * radius, 0, bossPosition.z + Mathf.Sin(angle) * radius);

                EntityManager.SetComponentData(spawnedEntity, new Translation { Value = newPos });

                commandBuffer.AddComponent(spawnedEntity, new ProjectileTag { damage = 50 });

                commandBuffer.AddComponent(spawnedEntity, new WaterNovaMovementData { duration = 5, direction = -(bossPosition - newPos), channelingTime = 5, speed = 10, angle = angle, radius = radius, height = 0 });


            }

            radius = radius - 0.5f;

            for (int i = 0; i < 8; i++)
            {

                //On accède à notre entity grace à la variable globale du PrefabEntityComponent
                Entity spawnedEntity = EntityManager.Instantiate(PrefabEntityComponent.waterDropPrefabEntity);

                float angle = i * Mathf.PI * 2f / 8;
                Vector3 newPos = new Vector3(bossPosition.x + Mathf.Cos(angle) * radius, 1, bossPosition.z + Mathf.Sin(angle) * radius);

                EntityManager.SetComponentData(spawnedEntity, new Translation { Value = newPos });

                commandBuffer.AddComponent(spawnedEntity, new ProjectileTag { damage = 50 });

                commandBuffer.AddComponent(spawnedEntity, new WaterNovaMovementData { duration = 5, direction = -(bossPosition - newPos), channelingTime = 5, speed = 10, angle = angle, radius = radius, height = 1 });


            }

            radius = radius - 0.5f;

            for (int i = 0; i < 8; i++)
            {

                //On accède à notre entity grace à la variable globale du PrefabEntityComponent
                Entity spawnedEntity = EntityManager.Instantiate(PrefabEntityComponent.waterDropPrefabEntity);

                float angle = i * Mathf.PI * 2f / 8;
                Vector3 newPos = new Vector3(bossPosition.x + Mathf.Cos(angle) * radius, 2, bossPosition.z + Mathf.Sin(angle) * radius);

                EntityManager.SetComponentData(spawnedEntity, new Translation { Value = newPos });

                commandBuffer.AddComponent(spawnedEntity, new ProjectileTag { damage = 50 });

                commandBuffer.AddComponent(spawnedEntity, new WaterNovaMovementData { duration = 5, direction = -(bossPosition - newPos), channelingTime = 5, speed = 10, angle = angle, radius = radius, height = 2 });


            }

            radius = radius - 0.5f;

            for (int i = 0; i < 8; i++)
            {

                //On accède à notre entity grace à la variable globale du PrefabEntityComponent
                Entity spawnedEntity = EntityManager.Instantiate(PrefabEntityComponent.waterDropPrefabEntity);

                float angle = i * Mathf.PI * 2f / 8;
                Vector3 newPos = new Vector3(bossPosition.x + Mathf.Cos(angle) * radius, 3, bossPosition.z + Mathf.Sin(angle) * radius);

                EntityManager.SetComponentData(spawnedEntity, new Translation { Value = newPos });

                commandBuffer.AddComponent(spawnedEntity, new ProjectileTag { damage = 50 });

                commandBuffer.AddComponent(spawnedEntity, new WaterNovaMovementData { duration = 5, direction = -(bossPosition - newPos), channelingTime = 5, speed = 10, angle = angle, radius = radius, height = 3 });


            }

            radius = radius - 0.5f;

            for (int i = 0; i < 8; i++)
            {

                //On accède à notre entity grace à la variable globale du PrefabEntityComponent
                Entity spawnedEntity = EntityManager.Instantiate(PrefabEntityComponent.waterDropPrefabEntity);

                float angle = i * Mathf.PI * 2f / 8;
                Vector3 newPos = new Vector3(bossPosition.x + Mathf.Cos(angle) * radius, 4, bossPosition.z + Mathf.Sin(angle) * radius);

                EntityManager.SetComponentData(spawnedEntity, new Translation { Value = newPos });

                commandBuffer.AddComponent(spawnedEntity, new ProjectileTag { damage = 50 });

                commandBuffer.AddComponent(spawnedEntity, new WaterNovaMovementData { duration = 5, direction = -(bossPosition - newPos), channelingTime = 5, speed = 10, angle = angle, radius = radius, height = 4 });

                
            }

            drap = false;

        }
      

        //spawnTimer = spawnTimer + Time.DeltaTime;

        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();

    
        return default;

    
    }
}
