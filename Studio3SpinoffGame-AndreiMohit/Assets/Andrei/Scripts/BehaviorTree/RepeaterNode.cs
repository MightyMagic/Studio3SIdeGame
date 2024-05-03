using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RepeaterNode : DecoratorNode
{
    protected override void OnStart()
    {
        //state = State.Running;
    }

    protected override void OnStop()
    {
       
    }

    protected override State OnUpdate()
    {
        //child.Update();
        //return State.Running;

        if(child.state == State.Success || child.state == State.Running)
        {
            child.Update();
            return State.Running;
        }
        else
        {
            return State.Failure;
        }
        
        //return State.Running;
    }
}
