using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Grass_dos_Stats : IComponentData
{
    public float vertex1X;
    public float vertex1Y;
    public float vertex1Z;
    public float vertex2X;
    public float vertex2Y;
    public float vertex2Z;
    public float vertex3X;
    public float vertex3Y;
    public float vertex3Z;
    public float vertex4X;
    public float vertex4Y;
    public float vertex4Z;
    public float vertex5X;
    public float vertex5Y;
    public float vertex5Z;
    public float vertex6X;
    public float vertex6Y;
    public float vertex6Z;
    public float vertex7X;
    public float vertex7Y;
    public float vertex7Z;
    public float vertex8X;
    public float vertex8Y;
    public float vertex8Z;

    public float windResistance;
    public float height;
    public Vector3 orientation;
}
