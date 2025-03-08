using UnityEngine;
using UnityEngine.UIElements;


// call UpdateText( ... ) to update the text in other scripts
public class SimpleText : MonoBehaviour
{
    public Label uiLabel;
    public string text; // initial text

    private WorldSpaceUIDocument worldSpaceUIDocument;

    private void Awake()
    {
        worldSpaceUIDocument = GetComponent<WorldSpaceUIDocument>();
    }
    private void Start()
    {
        uiLabel = worldSpaceUIDocument.uiDocument.rootVisualElement.Q<Label>("SimpleText");

        UpdateText(text); // set initial text
    }

    public void UpdateText(string newText)
    {
        if (uiLabel != null)
        {
            uiLabel.text = newText;
        }
    }

}
