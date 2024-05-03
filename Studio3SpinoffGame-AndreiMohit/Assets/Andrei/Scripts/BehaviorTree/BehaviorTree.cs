using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using UnityEditor;
using Unity.VisualScripting;
using System.Diagnostics;

[CreateAssetMenu()]
public class BehaviorTree : ScriptableObject
{
    public TreeNode rootNode;
    public TreeNode.State treeState = TreeNode.State.Running;

    public List<TreeNode> nodes = new List<TreeNode>();
    public float restartDelay;

    float timer = 0f;

    public TreeNode.State Update()
    {
        if(rootNode.state == TreeNode.State.Running)
        {
            treeState = rootNode.Update();
            return treeState;
        }
        else if(rootNode.state == TreeNode.State.Failure)
        {
            RestartTree();
        }

        return treeState;
    }

    public void RestartTree()
    {
        UnityEngine.Debug.LogError("Restart");
        // Reset the root node
        rootNode.state = TreeNode.State.Running;
        rootNode.started = false;

        // Reset all other nodes
        foreach (var node in nodes)
        {
            node.state = TreeNode.State.Running;
            node.started = false;
        }

        // Reset the tree state
        treeState = TreeNode.State.Running;

    }

#if UNITY_EDITOR

    public TreeNode CreateNode(System.Type type)
    {
        TreeNode node = ScriptableObject.CreateInstance(type) as TreeNode;
        node.name = type.Name;
        node.guid = UnityEditor.GUID.Generate().ToString();
        nodes.Add(node);

        UnityEditor.AssetDatabase.AddObjectToAsset(node, this);
        UnityEditor.AssetDatabase.SaveAssets();

        return node;
    }

    public void DeleteNode(TreeNode node)
    {
        nodes.Remove(node);

        UnityEditor.AssetDatabase.RemoveObjectFromAsset(node);

        UnityEditor.AssetDatabase.SaveAssets();
    }

    public void AddChild(TreeNode parent, TreeNode child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if(decorator)
        {
            decorator.child = child;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            composite.children.Add(child);
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            rootNode.child = child;
        }
    }

    public void RemoveChild(TreeNode parent, TreeNode child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            decorator.child = null;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            if(composite.children.Contains(child))
                composite.children.Remove(child);
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            rootNode.child = null;
        }
    }

    public List<TreeNode> GetChildren(TreeNode parent)
    {
        List<TreeNode> children = new List<TreeNode>();

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator && decorator.child != null)
        {
            children.Add(decorator.child);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            //children.AddRange(composite.children);
            return composite.children;
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode && rootNode.child != null)
        {
            children.Add(rootNode.child);
        }

        return children;
    }
#endif

    public BehaviorTree Clone()
    {
        BehaviorTree tree = Instantiate(this);
        tree.rootNode = tree.rootNode.Clone();

        return tree;
    }
}
