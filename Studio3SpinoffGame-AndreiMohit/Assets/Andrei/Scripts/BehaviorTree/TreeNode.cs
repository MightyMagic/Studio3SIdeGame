using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TreeNode : ScriptableObject
{
    public enum State
    {
        Running,
        Success,
        Failure
    }

    public State state = State.Running;
    public bool started = false;

    public string guid;
    public Vector2 position;

    public State Update()
    {
        if (!started)
        {
            started = true;
            OnStart();
        }

        state = OnUpdate();

        if(state == State.Failure || state == State.Success)
        {
            OnStop();
            started = false;
        }

        return state;   
    }

    public virtual TreeNode Clone()
    {
        return Instantiate(this);
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}
