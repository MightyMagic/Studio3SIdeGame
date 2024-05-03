using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : TreeNode
{
    public List<TreeNode> children;

    public override TreeNode Clone()
    {
        CompositeNode node = Instantiate(this);
        node.children = children.ConvertAll(c => c.Clone());

        return node;
    }
}
