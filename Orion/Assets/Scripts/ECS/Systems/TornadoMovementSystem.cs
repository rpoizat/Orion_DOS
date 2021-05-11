using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;

[AlwaysSynchronizeSystem]

[UpdateAfter(typeof(ActivateTornadoSystem))]
public class TornadoMovementSystem : JobComponentSystem
{

    // Start is called before the first frame update
    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
       
        float deltatime = Time.DeltaTime;

        // Ces variable vont servire à faire bouger nos sphère en cercles


        // requête pour accéder à toutes les entities qui ont un component translation, rotation et MovementData
        // La différence entre le mot clef ref et le mot clef in concerne la manière dont on accède à ces component
        // Ref on a droit à la lecture et à l'écriture
        // In veut dire qu'on ne peut que le lire.
        // On fait bouger les sphères de haut en bas

        EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

        Entities.ForEach((Entity e, ref Translation translation, ref Rotation rotation, ref TornadoMovementData tornadoMovementData) => {

            tornadoMovementData.timeCounter = tornadoMovementData.timeCounter + deltatime * tornadoMovementData.speed;

            translation.Value.x = tornadoMovementData.initialPos.x + (-1 * (Mathf.Cos(tornadoMovementData.timeCounter) *  tornadoMovementData.width));
            translation.Value.z = tornadoMovementData.initialPos.z + (tornadoMovementData.dropRotation * (Mathf.Sin(tornadoMovementData.timeCounter) * tornadoMovementData.height)); // ajouter -1 devant ou non pour changer sens de rotation


            tornadoMovementData.duration = tornadoMovementData.duration - deltatime;


            if (tornadoMovementData.duration <= 0)
            {
                commandBuffer.AddComponent<ExplosionTag>(e);
            }


        }).Run();

        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();

        return default;
    }
}
