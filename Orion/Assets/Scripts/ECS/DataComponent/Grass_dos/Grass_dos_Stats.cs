using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Grass_dos_Stats : IComponentData
{
    public float positionX;
    public float positionY;
    public float positionZ;
    public bool exist;
    public float windResistance;
    public float height;
}
