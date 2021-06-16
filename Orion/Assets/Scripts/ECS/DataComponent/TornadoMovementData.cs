using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;


[GenerateAuthoringComponent]
public struct TornadoMovementData : IComponentData
{


    //On va avoir besoin de sauvegarder la position initiale de la goutte d'eau pour avoir une constante qu'on va ajouter au script TornadoMovementSystem
    //Grace à cette constante on va pouvoir appliquer la fonction aux axe x et z
    //Sans cette constante tout va spawn au même endroit 
    public float3 initialPos;

    //un bool qui détermine si la goutte tourne de droite à gauche ou de gauche à droite
    public float dropRotation;

    //Le temps pendant lequel une goutte peut rester sur le terrain
    public float duration;

    public float timeCounter;
    public float speed;
    public float width;  
    public float height; 
    
}

