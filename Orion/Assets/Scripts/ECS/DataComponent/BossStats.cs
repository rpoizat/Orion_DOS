
using Unity.Entities;


// C'est un component vide que l'on va coller à notre boss pour l'appeler avec des queries

[GenerateAuthoringComponent]
public struct BossStats : IComponentData
{

    public int health;

    public float cdSpell1;

    public float cdSpell2;

    public float timeLeftSpell1;

    public float timeLeftSpell2;


}
