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
    ComputeBuffer index_buffer;
    Vector3[] data;
    int[] index;
    public UnityEngine.Material mat;

    private bool ascend = true;
 
    private void Start()
    {
        //variation = minFactor;
        eManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        listGrass = new Grass_dos_Stats[5];
        data = new Vector3[5 * 36];
        index = new int[5 * 36];
        InitialiseGrass();

        buffer = new ComputeBuffer(5 * 36, sizeof(float) * 3, ComputeBufferType.Default);
        index_buffer = new ComputeBuffer(5 * 36, sizeof(int), ComputeBufferType.Default);

        buffer.SetData(data);
        index_buffer.SetData(index);
        mat.SetBuffer("buffer", buffer);
        mat.SetBuffer("index", index_buffer);
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
        
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 5 * 36);
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

        for (float i = 0.0f; i < 10.0f; i += 2f)
        {
            Vector3 pos = new Vector3(i, 1.6f, 0.0f);
            if (cpt < 5)
            {
                CreateGrass(pos, cpt, convert);
                cpt++;
            }
        }
        settings.BlobAssetStore.Dispose();
    }

    private void CreateGrass(Vector3 position, int cpt, Entity c)
    {
        float hauteur = 1.0f; // UnityEngine.Random.Range(0.5f, 1.5f);
        //Vector3 orientation = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0.0f, UnityEngine.Random.Range(-1.0f, 1.0f));
        //orientation.Normalize();
        float thickness = 0.25f; //UnityEngine.Random.Range(0.008f, 0.01f);

        //générer la géométrie du brin en fonction de la position et de la hauteur
        //palier 1
        {
            Vector3 p1 = new Vector3(position.x + thickness, position.y, position.z);
            Vector3 p2 = new Vector3(position.x - thickness, position.y, position.z);
            Vector3 p3 = new Vector3(p1.x, p1.y + (1.0f / 4.0f) * hauteur, p1.z);
            Vector3 p4 = new Vector3(p2.x, p3.y, p2.z);
            Vector3 p5 = new Vector3(p1.x, p1.y + (2.0f / 4.0f) * hauteur, p1.z);
            Vector3 p6 = new Vector3(p2.x, p5.y, p2.z);
            Vector3 p7 = new Vector3(position.x + ((1.0f / 2.0f) * thickness), position.y + ((3.0f / 4.0f) * hauteur), position.z);
            Vector3 p8 = new Vector3(position.x - ((3.0f / 4.0f) * thickness), position.y + hauteur, position.z);

            data[(cpt * 8) + 0] = p1;
            data[(cpt * 8) + 1] = p2;
            data[(cpt * 8) + 2] = p3;
            data[(cpt * 8) + 3] = p4;
            data[(cpt * 8) + 4] = p5;
            data[(cpt * 8) + 5] = p6;
            data[(cpt * 8) + 6] = p7;
            data[(cpt * 8) + 7] = p8;
            //face 1
            index[(cpt * 36) + 0] = (cpt * 8 + 0);
            index[(cpt * 36) + 1] = (cpt * 8 + 1);
            index[(cpt * 36) + 2] = (cpt * 8 + 2);
                         
            index[(cpt * 36) + 3] = (cpt * 8 + 0);
            index[(cpt * 36) + 4] = (cpt * 8 + 2);
            index[(cpt * 36) + 5] = (cpt * 8 + 1);
                   
            //face 2
            index[(cpt * 36) + 6] = (cpt * 8 + 1);
            index[(cpt * 36) + 7] = (cpt * 8 + 2);
            index[(cpt * 36) + 8] = (cpt * 8 + 3);
                         
            index[(cpt * 36) + 9] = (cpt * 8 + 1);
            index[(cpt * 36) + 10] = (cpt * 8 + 3);
            index[(cpt * 36) + 11] = (cpt * 8 + 2);

            //face 3
            index[(cpt * 36) + 12] = (cpt * 8 + 2);
            index[(cpt * 36) + 13] = (cpt * 8 + 3);
            index[(cpt * 36) + 14] = (cpt * 8 + 4);

            index[(cpt * 36) + 15] = (cpt * 8 + 2);
            index[(cpt * 36) + 16] = (cpt * 8 + 4);
            index[(cpt * 36) + 17] = (cpt * 8 + 3);

            //face 4
            index[(cpt * 36) + 18] = (cpt * 8 + 3);
            index[(cpt * 36) + 19] = (cpt * 8 + 4);
            index[(cpt * 36) + 20] = (cpt * 8 + 5);

            index[(cpt * 36) + 21] = (cpt * 8 + 3);
            index[(cpt * 36) + 22] = (cpt * 8 + 5);
            index[(cpt * 36) + 23] = (cpt * 8 + 4);

            //face 5
            index[(cpt * 36) + 24] = (cpt * 8 + 4);
            index[(cpt * 36) + 25] = (cpt * 8 + 5);
            index[(cpt * 36) + 26] = (cpt * 8 + 6);

            index[(cpt * 36) + 27] = (cpt * 8 + 4);
            index[(cpt * 36) + 28] = (cpt * 8 + 6);
            index[(cpt * 36) + 29] = (cpt * 8 + 5);

            //face 6
            index[(cpt * 36) + 30] = (cpt * 8 + 5);
            index[(cpt * 36) + 31] = (cpt * 8 + 6);
            index[(cpt * 36) + 32] = (cpt * 8 + 7);

            index[(cpt * 36) + 33] = (cpt * 8 + 5);
            index[(cpt * 36) + 34] = (cpt * 8 + 7);
            index[(cpt * 36) + 35] = (cpt * 8 + 6);
        }       

        var brin = eManager.Instantiate(c);
        eManager.SetName(brin, "brin" + cpt);
        eManager.SetComponentData<Grass_dos_Stats>(brin, new Grass_dos_Stats {positionX = position.x, positionY = position.y, positionZ = position.z, exist = true, height = hauteur, windResistance = UnityEngine.Random.Range(0.8f, 1.0f)});
        eManager.SetComponentData(brin, new Translation { Value = position });
    }
}