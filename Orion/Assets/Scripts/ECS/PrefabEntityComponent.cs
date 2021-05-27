
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class PrefabEntityComponent : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public static Entity prefabEntity;

    public static Entity waterDropPrefabEntity;

    public GameObject prefabGameObject;

    public GameObject waterDropPrefabGameObject;

    //gameobject utilisé pour l'entité "goutte d'eau"


    public void Convert(Entity entity, EntityManager leManager, GameObjectConversionSystem conversionSystem)
    {
        // conversion de notre prefab game object en entity
        Entity prefabConvertiEntity = conversionSystem.GetPrimaryEntity(prefabGameObject);

        Entity waterDropConvertEntity = conversionSystem.GetPrimaryEntity(waterDropPrefabGameObject);

        // On stock ensuite le prefab converti en entity dans la variable static prefabEntity pour pouvoir y acceder de n'imoporte où
        PrefabEntityComponent.prefabEntity = prefabConvertiEntity;

        PrefabEntityComponent.waterDropPrefabEntity = waterDropConvertEntity;
    }


    // il semblerait qu'un prefab doive faire parti de la liste referencedPrefabs pour être converti 
    // No sabemos porque lol
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        //On y ajoute donc notre prefab au format gameObject
        referencedPrefabs.Add(prefabGameObject);

        referencedPrefabs.Add(waterDropPrefabGameObject);
    }



}
