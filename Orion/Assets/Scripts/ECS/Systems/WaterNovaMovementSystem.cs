using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using System.Numerics;

[AlwaysSynchronizeSystem]

[UpdateAfter(typeof(ActivateWaterNovaSystem))]

public class WaterNovaMovementSystem : JobComponentSystem
{
    // Start is called before the first frame update
    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        float deltatime = Time.DeltaTime;


        EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);
        Translation bossTranslation = new Translation();

        Entities.ForEach((ref Translation laBossTranslation, ref BossStats bossPosition) => {

            bossTranslation = laBossTranslation;

        }).Run();


        Entities.ForEach((Entity e, ref Translation translation, ref Rotation rotation, ref WaterNovaMovementData waterNovaMovementData) => {

            waterNovaMovementData.channelingTime = waterNovaMovementData.channelingTime - deltatime;

            if (waterNovaMovementData.duration > 0 && waterNovaMovementData.channelingTime <= 0)
            {
                waterNovaMovementData.timeCounter = waterNovaMovementData.timeCounter + deltatime * waterNovaMovementData.speed;

                waterNovaMovementData.direction = waterNovaMovementData.direction.normalized;

                translation.Value.x = waterNovaMovementData.direction.x * waterNovaMovementData.timeCounter;

                translation.Value.y = waterNovaMovementData.direction.y * waterNovaMovementData.timeCounter;

                translation.Value.z = waterNovaMovementData.direction.z * waterNovaMovementData.timeCounter;

                waterNovaMovementData.duration = waterNovaMovementData.duration - deltatime;

            }

            else
            {
                if (waterNovaMovementData.duration <= 0)
                {
                    commandBuffer.AddComponent<ExplosionTag>(e);
                }

                else
                {

                    UnityEngine.Vector3 newPos = new UnityEngine.Vector3(bossTranslation.Value.x + Mathf.Cos(waterNovaMovementData.angle) * waterNovaMovementData.radius, waterNovaMovementData.height, bossTranslation.Value.z + Mathf.Sin(waterNovaMovementData.angle) * waterNovaMovementData.radius);

                    commandBuffer.SetComponent(e, new Translation { Value = newPos });
                }


            }


        }).Run();

        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();

        return default;
    }
}
