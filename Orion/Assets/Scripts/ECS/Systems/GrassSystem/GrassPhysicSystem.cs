using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;

public class GrassPhysicSystem : JobComponentSystem
{
    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {


        return default;
    }
}
