using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SequenceNode : CompositeNode
{
    int current;

    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        var child = this.children[current];

        Debug.Log("Sequence current is " + current);

        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Success:
                current++;
                // Adding this for the game to work, so technically it's not a regular sequence, but an endless sequence. Will fix in the future
                if (current == children.Count)
                    Reset();
                //
                break;
            case State.Failure:
                return State.Failure;
        }

        return current == children.Count ? State.Success : State.Running;
    }

    public void Reset()
    {
        current = 0;
    }
}
