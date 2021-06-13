using System.Collections.Generic;

public class Sequence : Nodes
{
    private List<Nodes> nodeList = new List<Nodes>();
    
    public void AddNode(Nodes newNode) {
        
        nodeList.Add(newNode);
    }

    public void ClearList() {
        
        nodeList.Clear();
    }
    
    public override states Execute()
    {
        
        state = states.Running;
        foreach (Nodes node in nodeList)
        {
            state = node.Execute();
            if (state == states.Failure)
            {
                break;
            }
        }
        return state;
    }
    
    public override states Initialize()
    {
        foreach (Nodes node in nodeList)
        {
            node.Initialize();
            
        }
        
        
        return base.Initialize();
    }
}