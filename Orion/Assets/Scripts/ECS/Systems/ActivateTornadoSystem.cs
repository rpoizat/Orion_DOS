using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;


public class ActivateTornadoSystem : ComponentSystem
{
    private float spawnTimer = 0f;
    private Unity.Mathematics.Random random;
    private float3 position;

    protected override void OnCreate()
    {
        random = new Unity.Mathematics.Random(56);
    }
    protected override void OnUpdate()
    {
        
        if (spawnTimer >= 5f)
        {

            position = new float3(random.NextFloat(-50f, 50f), 0f, random.NextFloat(-50f, 50f));

            spawnTimer = 0;
            
            bool drap = true;
            for (int i = 0; i < 500; i = i + 1)
            {
                
                //On accède à notre entity grace à la variable globale du PrefabEntityComponent
                Entity spawnedEntity = EntityManager.Instantiate(PrefabEntityComponent.waterDropPrefabEntity);

                EntityManager.SetComponentData(spawnedEntity, new Translation { Value = new float3(position.x, position.y +( 0.10f * i), position.z) });


                
                if (drap)
                {
                    EntityManager.SetComponentData(spawnedEntity, new TornadoMovementData { duration = 15, initialPos = position, dropRotation = 1, timeCounter = 0f, speed = random.NextFloat(1,5), width = random.NextFloat(0.6f, 1f) * (1f + (0.05f * i)) , height = 7f });; ;

                }
                else
                {
                    EntityManager.SetComponentData(spawnedEntity, new TornadoMovementData { duration = 15, initialPos = position, dropRotation = -1, timeCounter = 0f, speed = random.NextFloat(1, 5), width = random.NextFloat(0.6f, 1f) * (1f + (0.05f * i)), height = 7f }); ;

                }

                drap = !drap;

            }



        }

        spawnTimer = spawnTimer + Time.DeltaTime;

    }

}
