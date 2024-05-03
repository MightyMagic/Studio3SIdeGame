using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    int currentChild;
    protected override void OnStart()
    {
        currentChild = 0;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        while (currentChild < children.Count)
        {
            var child = children[currentChild];
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Success:
                    return State.Success; 
                case State.Failure:
                    currentChild++;
                    break;
            }
        }

        return State.Failure; 
    }
}
