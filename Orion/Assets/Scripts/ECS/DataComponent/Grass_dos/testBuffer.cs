using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBuffer : MonoBehaviour
{
    ComputeBuffer buffer;
    public Material mat;

    // Start is called before the first frame update
    void Start()
    {
        buffer = new ComputeBuffer(6, sizeof(float) * 3, ComputeBufferType.Default);

        Vector3[] pos = new Vector3[6];
        pos[0] = new Vector3(0.0f, 0.0f, 0.0f);
        pos[2] = new Vector3(1.0f, 0.0f, 0.0f);
        pos[1] = new Vector3(0.0f, 1.0f, 0.0f);

        pos[3] = new Vector3(0.0f, 2.0f, 0.0f);       
        pos[5] = new Vector3(1.0f, 2.0f, 0.0f);
        pos[4] = new Vector3(0.0f, 3.0f, 0.0f);

        buffer.SetData(pos);
    }

    private void OnPostRender()
    {
        mat.SetPass(0);
        mat.SetBuffer("buffer", buffer);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 6);
    }

    private void OnDestroy()
    {
        buffer.Release();
    }
}
