using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractTextUI : MonoBehaviour
{
    [SerializeField] string interactKey;
    [SerializeField] string interactLabel;
    private VisualElement visualElement;

    private BoxCollider boxCollider;
    private WorldSpaceUIDocument worldSpaceUIDocument;

    private Transform player;

    
    private void Awake()
    {
        worldSpaceUIDocument = GetComponent<WorldSpaceUIDocument>();

    }
    private void Start()
    {
        worldSpaceUIDocument.SetLabelText("InteractKey", interactKey);
        worldSpaceUIDocument.SetLabelText("InteractLabel", interactLabel);

        visualElement = worldSpaceUIDocument.uiDocument.rootVisualElement.Q<VisualElement>("InteractTextContainer");
        if (visualElement == null)
        {
            Debug.LogError("visualElement null");
        }
        UIHelper.HideUI(visualElement);

        player = NetworkManager.Singleton.LocalClient.PlayerObject.transform;

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter trigger");
        if (other.transform == player)
        {
            UIHelper.ShowUI(visualElement);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit trigger");
        if (other.transform == player)
        {
            UIHelper.HideUI(visualElement);
        }
    }
    

}
