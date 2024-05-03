using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FailNode : LeafNode
{
    bool wasHere;
    protected override void OnStart()
    {
        wasHere = true;
    }

    protected override void OnStop()
    {
       wasHere = false;
    }

    protected override State OnUpdate()
    {
        if (wasHere)
        {
            return State.Failure;
        }
        else { return State.Running; }
    }
}
