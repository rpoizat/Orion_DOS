using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;


[GenerateAuthoringComponent]
public struct WaterNovaMovementData : IComponentData
{

    //vecteur qui donnera la direction vers laquelle la goutte sera projetée
    public Vector3 direction;

    //Le temps pendant lequel une goutte peut rester sur le terrain
    public float duration;

    // Temps pendant lequel la goutte restera en suspension avant d'être projetée
    public float channelingTime;

    // Vitesse à laquelle la goutte se déplacera
    public float speed;

    public float timeCounter;

    public float angle;

    public float radius;

    public float height;

}


