using Unity.Entities;

[GenerateAuthoringComponent]
public struct Grass_dos_Stats : IComponentData
{
    public float positionX;
    public float positionY;
    public float positionZ;
    public bool exist;
    public float windResistance;
    public float height;
    public float forceX;
    public float forceY;
    public float forceZ;
    public bool isStepOn;
}
