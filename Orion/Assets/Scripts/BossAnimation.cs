using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    [SerializeField] private ConvertedEntityHolder convertedEntityHolder;
    [SerializeField] private ConvertedEntityHolder convertedEntityHolderPlayer;
    [SerializeField] private GameObject bossGO;

    private Translation playerPos;

    private void Start()
    {
        bossGO = Instantiate(bossGO);
        bossGO.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    private void Update()
    {
        
        /*if(convertedEntityHolder == null)
            return;
        
        
        Translation t = convertedEntityHolder.entityManager.GetComponentData<Translation>(convertedEntityHolder.entity);
        */

        playerPos = convertedEntityHolderPlayer.entityManager.GetComponentData<Translation>(convertedEntityHolderPlayer.entity);
        
        bossGO.transform.LookAt(playerPos.Value);
        
        
    }
}
