using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics;

public class MeshShaderPlugin : MonoBehaviour
{
    [DllImport("MeshShaderPlugin")]
    static extern IntPtr Execute();
    EntityManager eManager;
    private GameObjectConversionSettings settings;
    GrassStats[] listGrass;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject prefabBrin;
    [SerializeField] private Vector3 ventMax;
    [SerializeField] private float minFactor;
    [SerializeField] private float variation;

    private bool ascend = true;
 
    private void Start()
    {
        //variation = minFactor;
        //eManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        //listGrass = new GrassStats[10000];
        //InitialiseGrass();
        StartCoroutine("CallNativePlugin");
    }

    /*IEnumerator Start()
    {
        variation = minFactor;
        eManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        listGrass = new GrassStats[10000];
        InitialiseGrass();
        yield return StartCoroutine("CallNativePlugin");
    }*/
 
    IEnumerator CallNativePlugin()
    {
        while (true)
        {
            //phase ascendante du vent
            if(ascend)
            {
                if(variation > 0.9f)
                {
                    //accélération ralentie sur la fin
                    variation += 0.001f;
                }
                else
                {
                    //accélération normale
                    variation += 0.005f;
                }

                if (variation > 1.0f) ascend = false;
            }
            else
            {
                //phase descendante du vent
                if(variation < minFactor + 0.1f)
                {
                    //décélération ralentie sur la fin
                    variation -= 0.001f;
                }
                else
                {
                    //décélération normale
                    variation -= 0.005f;
                }

                if (variation < minFactor) ascend = true;
            }

            //calcul de la matrice ViewProjection
            var viewProjection = mainCamera.nonJitteredProjectionMatrix * mainCamera.transform.worldToLocalMatrix;

            yield return new WaitForEndOfFrame();
            GL.IssuePluginEvent(Execute(), 1);
        }
    }

    //récupérer les données des brins d'herbes
    private void Update()
    {
        /*EntityQuery query = eManager.CreateEntityQuery(ComponentType.ReadOnly<GrassTag>());
        var res = query.ToEntityArray(Unity.Collections.Allocator.TempJob);

        res.Dispose();*/

        /*for(int i = 0; i < res.Length; i++)
        {
            if(i < 10000) listGrass[i] = eManager.GetComponentData<GrassStats>(res[i]);
        }*/
    }

    private void InitialiseGrass()
    {
        int cpt = 0;
        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore());
        var convert = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefabBrin, settings);

        for (float i = 0.0f; i < 1.0f; i += 0.01f)
        {
            for(float j = 0.0f; j < 1.0f; j += 0.01f)
            {
                Vector3 pos = Vector3.zero;
                pos.x += (i + UnityEngine.Random.Range(-0.005f, 0.005f));
                pos.y = 1.6f;
                pos.z += (j + UnityEngine.Random.Range(-0.005f, 0.005f));
                CreateGrass(pos, cpt, convert);
                cpt++;
            }
        }
        settings.BlobAssetStore.Dispose();
    }

    private void CreateGrass(Vector3 position, int cpt, Entity c)
    {
        //float hauteur = UnityEngine.Random.Range(0.5f, 1.5f);
        float hauteur = UnityEngine.Random.Range(0.0f, 0.2f);
        position.y += hauteur;
        var brin = eManager.Instantiate(c);
        eManager.SetName(brin, "brin" + cpt);
        eManager.SetComponentData<GrassStats>(brin, new GrassStats {positionX = position.x, positionY = position.y, positionZ = position.z, hauteur = hauteur, resistance = UnityEngine.Random.Range(0.8f, 1.0f), orientationX = 0.0f, orientationY = 1.0f, orientationZ = 0.0f });
        eManager.SetComponentData(brin, new Translation { Value = position });
    }
}