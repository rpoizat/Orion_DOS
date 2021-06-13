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
public class NewBehaviourScript : JobComponentSystem
{
    // Start is called before the first frame update
    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        float deltatime = Time.DeltaTime;

        Vector3 bossPosition = new Vector3();

        // Ces variable vont servire à faire bouger nos sphère en cercles


        // requête pour accéder à toutes les entities qui ont un component translation, rotation et MovementData
        // La différence entre le mot clef ref et le mot clef in concerne la manière dont on accède à ces component
        // Ref on a droit à la lecture et à l'écriture
        // In veut dire qu'on ne peut que le lire.
        // On fait bouger les sphères de haut en bas

        EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

        Entities.ForEach((ref BossStats lesCD, ref Translation bossPos) => {

            if(lesCD.timeLeftSpell1 > 0)
            {
                lesCD.timeLeftSpell1 = lesCD.timeLeftSpell1 - deltatime;
            }

            if (lesCD.timeLeftSpell2 > 0)
            {
                lesCD.timeLeftSpell2 = lesCD.timeLeftSpell2 - deltatime;
            }

            bossPosition = new Vector3 (bossPos.Value.x, bossPos.Value.y, bossPos.Value.z);

            WaterBossBehaviourTree._instance.timeLeftSpell1 = lesCD.timeLeftSpell1;
            WaterBossBehaviourTree._instance.timeLeftSpell2 = lesCD.timeLeftSpell2;

            WaterBossBehaviourTree._instance.cdSpell1 = lesCD.cdSpell1;
            WaterBossBehaviourTree._instance.cdSpell2 = lesCD.cdSpell2;



        }).Run();

        Entities.ForEach((ref PlayerTag playerTag , ref Translation playerPos) => {

            WaterBossBehaviourTree._instance.rangePlayerBoss = Vector3.Distance(bossPosition, new Vector3(playerPos.Value.x, playerPos.Value.y, playerPos.Value.z));

        }).Run();





        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();

        return default;
    }
}
