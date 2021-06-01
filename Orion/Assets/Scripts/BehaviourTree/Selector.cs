using System.Collections.Generic;

public class Selector : Nodes
{
    private List<Nodes> nodeList = new List<Nodes>();

    public void AddNode(Nodes newNode)
    {
        nodeList.Add(newNode);
    }

    public void ClearList()
    {
        nodeList.Clear();
    }

    public override states Execute()
    {
        state = states.Running;
        /*do
        {*/
            foreach (Nodes node in nodeList)
            {
                
                state = node.Execute();

                if (state == states.Success || state == states.Running)
                {
                    break;
                }
                
            }
        //} while (state == states.Running);
        
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