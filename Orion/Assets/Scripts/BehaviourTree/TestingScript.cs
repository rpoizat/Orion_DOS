using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    
    [SerializeField] private bool _motivated = false;
    [SerializeField] private bool _gotTime = false;
    
    
    [SerializeField] private float cookingTime = 10.0f;
    [SerializeField] private float cookingCounter = 0.0f;
    
    
    [SerializeField] private float executeTime = 1.0f;
    [SerializeField] private float executeCounter = 0.0f;


    private readonly Sequence behaviourTree = new Sequence();
    
    private states OrderFood()
    {
        print("I order food");
        return states.Success;
    }

    private states CookFood()
    {
        print("I cook food");
        cookingCounter += 1.0f;
        if (cookingCounter >= cookingTime)
        {
            print("food is cooked");
            cookingCounter = 0.0f;
            return states.Success;
        }
        return states.Running;
    }

    private states isMotivated()
    {
        if (_motivated)
            return states.Success;
        return states.Failure;
    }

    private states haveTime()
    {
        if (_gotTime)
            return states.Success;
        return states.Failure;   
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Nodes motivationCondition = new Nodes(isMotivated, baseNodeType.Condition);
        Nodes timeCondition = new Nodes(haveTime, baseNodeType.Condition);
        Nodes cookAction = new Nodes(CookFood, baseNodeType.Action);
        
        Sequence cookingSequence = new Sequence();
        cookingSequence.AddNode(motivationCondition);
        cookingSequence.AddNode(timeCondition);
        cookingSequence.AddNode(cookAction);
        
        Nodes orderAction = new Nodes(OrderFood, baseNodeType.Action);
        
        Selector orderSelector = new Selector();
        
        orderSelector.AddNode(cookingSequence);
        orderSelector.AddNode(orderAction);
        
        behaviourTree.AddNode(orderSelector);

        
    }

    private IEnumerator excuteTree()
    {
        yield return new WaitForSeconds(5.5f);
        
    }

    private void Update()
    {
        executeCounter += Time.deltaTime;
        if (executeCounter >= executeTime)
        {
            states result = behaviourTree.Execute();
            behaviourTree.Initialize();
            executeCounter = 0.0f;
        }
        
    }
}
