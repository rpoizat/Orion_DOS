using Unity.Entities;


// C'est un component vide que l'on va coller à notre boss pour l'appeler avec des queries

[GenerateAuthoringComponent]
public struct ProjectileTag : IComponentData
{

    public int damage;

}
