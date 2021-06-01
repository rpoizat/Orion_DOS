using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum states
{
    NotExecuted,
    Running,
    Failure,
    Success
}

public enum baseNodeType
{
    Condition,
    Action
}


//BASENODE
public class Nodes
{

    private Func<states> _action;
    
    //Spécifique pour la class de base Condition ou Action
    private baseNodeType _nodeType;
    
    //L'état à l'instant t du Noeud
    public states state = states.NotExecuted;
    

    //Constructeur par défaut
    protected Nodes() { }
    
    //Constructeur du Noeud de base
    public Nodes(Func<states> f, baseNodeType newNodeType)
    {
        _action = f;
        _nodeType = newNodeType;
    }

    //Methode virtuel de l'exécution de l'action lié au Noeud
    public virtual states Execute()
    {
        state = _action();
        return state;
    }

    //Methode virtuel de la réinitialisation lié au Noeud
    public virtual states Initialize()
    {
        state = states.NotExecuted;
        return state;
    }
    
}


