using UnityEngine;
using UnityEngine.UIElements;

public static class UIHelper
{
    public static void ShowUI(GameObject uiGameObject, string containerName)
    {
        VisualElement element = uiGameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>(containerName);
        element.style.visibility = Visibility.Visible;
    }

    public static void HideUI(GameObject uiGameObject, string containerName)
    {
        VisualElement element = uiGameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>(containerName);
        element.style.visibility = Visibility.Hidden;
    }

    // Returns reference to a UI element within a UIDocument
    public static T GetUIElement<T>(GameObject gameObject, string name) where T : VisualElement
    {
        if (gameObject == null)
            Debug.LogError($"{gameObject.name} not found");
        else if (gameObject.GetComponent<UIDocument>().rootVisualElement.Q<T>(name) == null)
            Debug.LogError($"{name} not found on {gameObject}'s UIDocument");

        return gameObject.GetComponent<UIDocument>().rootVisualElement.Q<T>(name);

    }
}
