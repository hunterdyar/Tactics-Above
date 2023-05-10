using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Texture2DEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private Texture2D _texture2D;
    [MenuItem("Window/UI Toolkit/Texture2DEditor")]
    public static void ShowExample()
    {
        Texture2DEditor wnd = GetWindow<Texture2DEditor>();
        wnd.titleContent = new GUIContent("Texture2DEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // VisualElement gridimage = ();

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }
}
