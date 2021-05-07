using Unity.Entities;
using UnityEngine;
[GenerateAuthoringComponent]
public struct GrassStats : IComponentData
{
    public float positionX;
    public float positionY;
    public float positionZ;
    public float orientationX;
    public float orientationY;
    public float orientationZ;
    public float hauteur;
    public float resistance;
}
