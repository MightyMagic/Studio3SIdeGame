using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviorTreeEditorNew : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/BehaviorTreeEditorNew")]
    public static void ShowExample()
    {
        BehaviorTreeEditorNew wnd = GetWindow<BehaviorTreeEditorNew>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditorNew");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        //var styleSheet = AssetDatabase.
    }
}
