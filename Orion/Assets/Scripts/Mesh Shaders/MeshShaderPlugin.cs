using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics;

public class MeshShaderPlugin : MonoBehaviour
{
    EntityManager eManager;
    private GameObjectConversionSettings settings;
    Grass_dos_Stats[] listGrass;

    //[SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject prefabBrin;
    [SerializeField] private Vector3 ventMax;
    [SerializeField] private float intensite;
    private float variation;

    ComputeBuffer buffer;
    ComputeBuffer index_buffer;
    ComputeBuffer windResistance;
    ComputeBuffer force_buffer;

    Vector3[] data;
    int[] index;
    float[] windR;
    public UnityEngine.Material mat;
    public UnityEngine.Material contours;
    public int nbbrins;
 
    private void Start()
    {
        Camera.onPostRender += OnPostRenderCallback;

        //variation = minFactor;
        eManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        listGrass = new Grass_dos_Stats[nbbrins];
        data = new Vector3[nbbrins * 36];
        index = new int[nbbrins * 36];
        windR = new float[nbbrins];
        InitialiseGrass();

        buffer = new ComputeBuffer(nbbrins * 36, sizeof(float) * 3, ComputeBufferType.Default);
        index_buffer = new ComputeBuffer(nbbrins * 36, sizeof(int), ComputeBufferType.Default);
        windResistance = new ComputeBuffer(nbbrins, sizeof(float), ComputeBufferType.Default);
        force_buffer = new ComputeBuffer(nbbrins, sizeof(float) * 3, ComputeBufferType.Default);

        buffer.SetData(data);
        index_buffer.SetData(index);
        windResistance.SetData(windR);

        Shader.SetGlobalBuffer("buffer", buffer);
        Shader.SetGlobalBuffer("index", index_buffer);
        Shader.SetGlobalBuffer("windResistance", windResistance);

        data = null;
        index = null;
        windR = null;
    }

    private void OnPostRenderCallback(Camera cam)
    {
        variation = (Mathf.Sin(Time.time) / intensite) + 0.3f;
        Vector3 v = variation * ventMax.normalized;
        mat.SetVector("wind", v);
        mat.SetPass(0);
        
        Graphics.DrawProceduralNow(MeshTopology.Triangles, nbbrins * 36);

        contours.SetVector("wind", v);
        contours.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Lines, nbbrins * 36);
    }

    private void OnDestroy()
    {
        buffer.Release();
        index_buffer.Release();
        windResistance.Release();
        force_buffer.Release();

        Camera.onPostRender -= OnPostRenderCallback;
    }

    private void InitialiseGrass()
    {
        int cpt = 0;
        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore());
        var convert = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefabBrin, settings);

        for (float i = -100.0f; i < 100.0f; i += 0.1f)
        {
            for(float j = -50.0f; j < 50.0f; j += 0.1f)
            {
                float x = i + UnityEngine.Random.Range(0.05f, 0.2f);
                float z = j + UnityEngine.Random.Range(0.05f, 0.2f);

                Vector3 pos = new Vector3(x, -0.5f, z);
                if (cpt < nbbrins)
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
        float hauteur = UnityEngine.Random.Range(0.2f, 1.0f);
        Vector3 orientation = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0.0f, UnityEngine.Random.Range(-1.0f, 1.0f));
        orientation.Normalize();
        float thickness = UnityEngine.Random.Range(0.01f, 0.05f);

        //générer la géométrie du brin en fonction de la position et de la hauteur
        //palier 1
        {
            Vector3 d1 = thickness * orientation;
            Vector3 d2 = (0.5f * thickness) * orientation;
            Vector3 p1 = position + d1;
            Vector3 p2 = position - d1;
            Vector3 p3 = new Vector3(p1.x, p1.y + (1.0f / 4.0f) * hauteur, p1.z);
            Vector3 p4 = new Vector3(p2.x, p3.y, p2.z);
            Vector3 p5 = new Vector3(p1.x, p1.y + (2.0f / 4.0f) * hauteur, p1.z);
            Vector3 p6 = new Vector3(p2.x, p5.y, p2.z);
            Vector3 p7 = position + d2;
            p7.y += (3.0f / 4.0f) * hauteur;
            Vector3 p8 = new Vector3(position.x, position.y + hauteur, position.z);

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
        float wr = UnityEngine.Random.Range(0.2f, 2.0f);
        windR[cpt] = wr;
        eManager.SetComponentData<Grass_dos_Stats>(brin, new Grass_dos_Stats {positionX = position.x, positionY = position.y, positionZ = position.z, exist = true, height = hauteur, windResistance = wr});
        eManager.SetComponentData(brin, new Translation { Value = position });
    }
}