using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;

public class GrassPhysicSystem : JobComponentSystem
{
    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Vector3 playerPosition = Vector3.zero;

        //récupérer la postion du joueur
        Entities.ForEach((in Translation trans, in PlayerTag player_tag) =>
        {
            playerPosition = trans.Value;
        }).Run();

        //traiter chaque brin
        Entities.ForEach((ref Grass_dos_Stats data_brin) =>
        {
            Vector3 positionBrin = new Vector3(data_brin.positionX, data_brin.positionY, data_brin.positionZ);

            if (Vector3.Distance(playerPosition, positionBrin) < 1.0f)
            {
                data_brin.isStepOn = true;
                Vector3 force = positionBrin - playerPosition;
                force.y = -data_brin.height;

                data_brin.forceX = force.x;
                data_brin.forceY = force.y;
                data_brin.forceZ = force.z;
            }
            else
            {
                //si l'herbe était pietinée mais ne l'est maintenant plus, il faut la redresser progressivement
                if(data_brin.isStepOn == true)
                {
                    //si le brin est complètement redressé, il n'est plus considéré comme étant marché dessus
                    if (data_brin.forceX == 0.0f && data_brin.forceY == 0.0f && data_brin.forceZ == 0.0f) data_brin.isStepOn = false;
                    else
                    {
                        //sinon, le faire se redresser
                        float factor = 1.0f;
                        factor -= 0.1f * (1.0f / data_brin.windResistance);

                        data_brin.forceX *= factor;
                        data_brin.forceY *= factor;
                        data_brin.forceZ *= factor;

                        if (Mathf.Abs(data_brin.forceX) < 0.1f) data_brin.forceX = 0.0f;
                        if (Mathf.Abs(data_brin.forceY) < 0.1f) data_brin.forceY = 0.0f;
                        if (Mathf.Abs(data_brin.forceZ) < 0.1f) data_brin.forceZ = 0.0f;
                    }
                }

                data_brin.isStepOn = false;
            }

        }).Run();

        return default;
    }
}
