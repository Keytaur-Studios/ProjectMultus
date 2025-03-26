using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;

/* 
 * This class allows you to place UI Toolkit UI Documents into World Space. 
 * 
 * How this script works:
 * 1. The script sets up a UIDocument with a RenderTexture.
 * 2. The RenderTexture is applied to a material.
 * 3. The material is displayed on a quad in world space.
 * 4. The UI can be interacted with via UIDocument.
*/


public class WorldSpaceUIDocument : MonoBehaviour
{
    // determine if the UI background should be transparent or not.
    private const string k_transparentShader = "Unlit/Transparent";
    private const string k_textureShader = "Unlit/Texture";
    private const string k_mainTex = "_MainTex";
    private static readonly int MainText = Shader.PropertyToID(k_mainTex); // used to apply the RenderTexture to a material

    [Header("Panel Configuration")]
    [SerializeField] int panelWidth = 1280;
    [SerializeField] int panelHeight = 720;
    [SerializeField] float panelScale = 1.0f;
    [SerializeField] float pixelsPerUnit = 500.0f;

    [Header("UI Assets")]
    public VisualTreeAsset visualTreeAsset; // UXML file for UI layout
    [SerializeField] PanelSettings panelSettingsAsset; 
    [SerializeField] RenderTexture renderTextureAsset; // RenderTexture used to display the UI

    private MeshRenderer meshRenderer; // displays UI onto a 3D object
    public UIDocument uiDocument;
    private PanelSettings panelSettings;
    private RenderTexture renderTexture; // UI is rendered onto this
    private Material material;


    // Finds a Label UI element by name and updates its text dynamically
    public void SetLabelText(string label, string text)
    {
        // make sure uiDocument is assigned
        if (uiDocument.rootVisualElement == null)
        {
            uiDocument.visualTreeAsset = visualTreeAsset;
        }

        uiDocument.rootVisualElement.Q<Label>(label).text = text;
    }
    private void Awake()
    {
        InitializeComponents();
        BuildPanel();
    }

    // Sets up the world-space UI
    void BuildPanel()
    {
        CreateRenderTexture();
        CreatePanelSettings();
        CreateUIDocument();
        CreateMaterial();

        SetMaterialToRenderer();
        SetPanelSize();
    }

    // Assigns the generated material to the MeshRenderer so that the UI appears on the quad.
    void SetMaterialToRenderer()
    {
        if (meshRenderer != null)
        {
            meshRenderer.sharedMaterial = material;
            Debug.Log("rendered!");
        }
    }


    void SetPanelSize()
    {
        // Resizes the RenderTexture to match the UI dimensions in the case the size 
        if (renderTexture != null && (renderTexture.width != panelWidth || renderTexture.height != panelHeight))
        {
            renderTexture.Release();
            renderTexture.width = panelWidth;
            renderTexture.height = panelHeight;
            renderTexture.Create();

            uiDocument?.rootVisualElement?.MarkDirtyRepaint();
        }

        // Adjusts the scale of the quad to maintain the correct aspect ratio in world space
        transform.localScale = new Vector3 (panelWidth / pixelsPerUnit, panelHeight / pixelsPerUnit, 1.0f);
    }

    // Creates a Material to display the RenderTexture on a quad
    void CreateMaterial()
    {
        // Determines whether to use a transparent or opaque shader based on the panel’s background color
        string shaderName = panelSettings.colorClearValue.a < 1.0f ? k_transparentShader : k_textureShader;
        // Assigns the RenderTexture to the material
        material = new Material(Shader.Find(shaderName));
        material.SetTexture(MainText, renderTexture);
    }

    // Attaches a UIDocument to the GameObject
    void CreateUIDocument()
    {
        uiDocument = gameObject.GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            uiDocument = gameObject.AddComponent<UIDocument>();
        }
        uiDocument.panelSettings = panelSettings; // linking the panelSettings will link this to the RenderTexture
        uiDocument.visualTreeAsset = visualTreeAsset;
    }

    // Links the UI system to the RenderTexture
    void CreatePanelSettings()
    {
        panelSettings = Instantiate(panelSettingsAsset);
        panelSettings.targetTexture = renderTexture; // makes sure UI elements render into this texture
        panelSettings.clearColor = true;
        panelSettings.scaleMode = PanelScaleMode.ConstantPixelSize;
        panelSettings.scale = panelScale;
        panelSettings.name = $"{name} - PanelSettings";
    }


    void CreateRenderTexture()
    {
        RenderTextureDescriptor descriptor = renderTextureAsset.descriptor;
        descriptor.width = panelWidth;
        descriptor.height = panelHeight;
        renderTexture = new RenderTexture(descriptor) {
            name = $"{name} - RenderTexture"
        };
    }

    void InitializeComponents()
    {
        InitializeMeshRenderer();

        // Create a meshFilter to assign a quad mesh
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        meshFilter.sharedMesh = GetQuadMesh();

    }


    // Configures mesh renderer (i.e., disables shadows and reflections (since it's just a UI panel))
    void InitializeMeshRenderer()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        meshRenderer.sharedMaterial = null;
        meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        meshRenderer.receiveShadows = false;
        meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        meshRenderer.lightProbeUsage = LightProbeUsage.Off;
        meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
    }

    // Helper method to generate a quad mesh
    static Mesh GetQuadMesh()
    {
        GameObject tempQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Mesh quadMesh = tempQuad.GetComponent<MeshFilter>().sharedMesh;
        Destroy(tempQuad);

        return quadMesh;
    }
}
