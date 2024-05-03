using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviorTreeEditor : EditorWindow
{

    BehaviorTreeView treeView;
    InspectorView inspectorView;

   //[SerializeField]
   //private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/BehaviorTreeEditor/Editor ...")]
    public static void OpenWindow()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }

    public void CreateGUI()
    {
        //Debug.LogError("I'm creating this window!");
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Clear root before adding new elements
        //root.Clear();

        // Instantiate UXML
        //VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        //root.Add(labelFromUXML);

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/OldVersion/BehaviorTreeEditor.uxml");
        visualTree.CloneTree(root);

        // Style sheet
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/OldVersion/BehaviorTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<BehaviorTreeView>();
        inspectorView = root.Q<InspectorView>();

        treeView.OnNodeSelected = OnNodeSelectionChanged;
        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        BehaviorTree tree = Selection.activeObject as BehaviorTree;

        if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
        {
            treeView.PopulateView(tree);
        }
    }

    void OnNodeSelectionChanged(NodeView node)
    {
        inspectorView.UpdateSelection(node);
    }
}
