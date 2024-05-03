using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class DebugNode : LeafNode
{

    public string message;

    protected override void OnStart()
    {
        Debug.Log("On start: " + message);
    }

    protected override void OnStop()
    {
        Debug.Log("On stop: " + message);
    }

    protected override State OnUpdate()
    {
        Debug.Log("On update: " + message);
        return TreeNode.State.Success;
    }
}
