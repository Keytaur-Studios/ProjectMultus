using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

// This class has methods to help manage UI Toolkit elements + methods
public static class UIHelper
{
    public static void ShowUI(GameObject uiGameObject, string containerName)
    {
        if (uiGameObject == null)
        {
            Debug.LogError($"{uiGameObject.name} is null");
        }

        VisualElement element = uiGameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>(containerName);
        element.style.visibility = Visibility.Visible;
    }
    public static void ShowUI(VisualElement element)
    {
        element.style.visibility = Visibility.Visible;
    }

    public static void HideUI(GameObject uiGameObject, string containerName)
    {
        if (uiGameObject == null)
        {
            Debug.LogError($"{uiGameObject.name} is null");
        }

        VisualElement element = uiGameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>(containerName);
        element.style.visibility = Visibility.Hidden;
    }

    public static void HideUI(VisualElement element)
    {
        element.style.visibility = Visibility.Hidden;
    }

    // Returns reference to a UI element within a UIDocument
    // Usage example: startGameButton = UIHelper.GetUIElement<Button>(lobbyMenuUI, "StartGameButton");
    public static T GetUIElement<T>(GameObject gameObject, string name) where T : VisualElement
    {
        // Null reference error handling
        if (gameObject == null)
            Debug.LogError($"{gameObject.name} not found");
        else if (gameObject.GetComponent<UIDocument>().rootVisualElement.Q<T>(name) == null)
            Debug.LogError($"{name} not found on {gameObject}'s UIDocument");

        return gameObject.GetComponent<UIDocument>().rootVisualElement.Q<T>(name);

    }

    public static void resetEventSystemFocus(VisualElement element)
    {
        EventSystem.current.SetSelectedGameObject(null);
        element.Focus();
    }
}
