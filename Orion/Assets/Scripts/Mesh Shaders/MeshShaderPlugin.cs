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
    Grass_dos_Stats[] listGrass;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject prefabBrin;
    [SerializeField] private Vector3 ventMax;
    [SerializeField] private float minFactor;
    [SerializeField] private float variation;

    ComputeBuffer buffer;
    Vector3[] data;
    public UnityEngine.Material mat;

    private bool ascend = true;
 
    private void Start()
    {
        //variation = minFactor;
        eManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        listGrass = new Grass_dos_Stats[2];
        data = new Vector3[2 * 36];
        InitialiseGrass();

        buffer = new ComputeBuffer(2 * 36, sizeof(float) * 3, ComputeBufferType.Default);
    

        buffer.SetData(data);
    }

    //récupérer les données des brins d'herbes
    private void Update()
    {
        //GESTION DU VENT
        //phase ascendante du vent
        if (ascend)
        {
            if (variation > 0.9f)
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
            if (variation < minFactor + 0.1f)
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
    }

    private void OnPostRender()
    {
        mat.SetPass(0);
        mat.SetBuffer("buffer", buffer);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 2 * 36);
    }

    private void OnDestroy()
    {
        buffer.Release();
    }

    private void InitialiseGrass()
    {
        int cpt = 0;
        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore());
        var convert = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefabBrin, settings);

        for (float i = 0.0f; i < 10.0f; i += 1f)
        {
            for(float j = 0.0f; j < 10.0f; j += 1f)
            {
                Vector3 pos = Vector3.zero;
                pos.x += (i + UnityEngine.Random.Range(-0.005f, 0.005f));
                pos.y = 1.6f;
                pos.z += (j + UnityEngine.Random.Range(-0.005f, 0.005f));

                if(cpt < 2)
                {
                    CreateGrass(pos, cpt, convert);
                    cpt++;
                }
            }
        }
        settings.BlobAssetStore.Dispose();
    }

    private void CreateGrass(Vector3 position, int cpt, Entity c)
    {
        float hauteur = UnityEngine.Random.Range(0.5f, 1.5f);
        Vector3 orientation = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0.0f, UnityEngine.Random.Range(-1.0f, 1.0f));
        orientation.Normalize();
        float thickness = 0.5f; //UnityEngine.Random.Range(0.008f, 0.01f);

        //générer la géométrie du brin en fonction de la position et de la hauteur
        //palier 1
        {
            Vector3 decal1 = orientation * thickness;
            Vector3 decal2 = orientation * thickness;

            Vector3 p1 = position + decal1;
            Vector3 p2 = position - decal2;
            Vector3 p3 = p1;
            p3.y += (1.0f / 4.0f) * hauteur;
            Vector3 p4 = p2;
            p4.y = p3.y;
            Vector3 p5 = p1;
            p5.y += (2.0f / 4.0f) * hauteur;
            Vector3 p6 = p2;
            p6.y = p5.y;
            Vector3 p7 = position;
            p7.x += (thickness /2.0f) * thickness;
            p7.y += (3.0f / 4.0f) * hauteur;
            Vector3 p8 = new Vector3(position.x, position.y + hauteur, position.z);

            //face 1
            data[(cpt * 36) + 0] = p1;
            data[(cpt * 36) + 1] = p2;
            data[(cpt * 36) + 2] = p3;
                         
            //face 1 back
            data[(cpt * 36) + 3] = p1;
            data[(cpt * 36) + 4] = p3;
            data[(cpt * 36) + 5] = p2;
                         
            //face 2     
            data[(cpt * 36) + 6] = p2;
            data[(cpt * 36) + 7] = p3;
            data[(cpt * 36) + 8] = p4;
                         
            //face 2 bac 
            data[(cpt * 36) + 9] = p2;
            data[(cpt * 36) + 10] = p4;
            data[(cpt * 36) + 11] = p3;
                         
            //face 3     
            data[(cpt * 36) + 12] = p3;
            data[(cpt * 36) + 13] = p4;
            data[(cpt * 36) + 14] = p5;
                         
            //face 3 bac 
            data[(cpt * 36) + 15] = p3;
            data[(cpt * 36) + 16] = p5;
            data[(cpt * 36) + 17] = p4;
                         
            //face 4     
            data[(cpt * 36) + 18] = p4;
            data[(cpt * 36) + 19] = p5;
            data[(cpt * 36) + 20] = p6;
                         
            //face 4 bac 
            data[(cpt * 36) + 21] = p4;
            data[(cpt * 36) + 22] = p6;
            data[(cpt * 36) + 23] = p5;
                         
            //face 5     
            data[(cpt * 36) + 24] = p5;
            data[(cpt * 36) + 25] = p6;
            data[(cpt * 36) + 26] = p7;
                         
            //face 5 bac 
            data[(cpt * 36) + 27] = p5;
            data[(cpt * 36) + 28] = p7;
            data[(cpt * 36) + 29] = p6;

            //face 6
            data[(cpt * 35) + 30] = p6;
            data[(cpt * 35) + 31] = p7;
            data[(cpt * 35) + 32] = p8;
                        
            //face 6 bac
            data[(cpt * 35) + 33] = p6;
            data[(cpt * 35) + 34] = p8;
            data[(cpt * 35) + 35] = p7;
        }       

        var brin = eManager.Instantiate(c);
        eManager.SetName(brin, "brin" + cpt);
        eManager.SetComponentData<Grass_dos_Stats>(brin, new Grass_dos_Stats {positionX = position.x, positionY = position.y, positionZ = position.z, exist = true, height = hauteur, windResistance = UnityEngine.Random.Range(0.8f, 1.0f)});
        eManager.SetComponentData(brin, new Translation { Value = position });
    }
}