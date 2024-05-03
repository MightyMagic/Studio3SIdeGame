using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorNode : TreeNode
{
    public TreeNode child;

    public override TreeNode Clone()
    {
        DecoratorNode node = Instantiate(this);
        node.child = this.child.Clone();

        return node;
    }
}
