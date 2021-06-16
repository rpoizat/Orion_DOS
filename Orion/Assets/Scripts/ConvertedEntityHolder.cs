using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ConvertedEntityHolder : MonoBehaviour, IConvertGameObjectToEntity
{
    [HideInInspector] public Entity entity;
    [HideInInspector] public EntityManager entityManager;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        this.entity = entity;
        this.entityManager = dstManager;
        Debug.Log(entity);
    }
}
