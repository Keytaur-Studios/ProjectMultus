using UnityEngine;

public class InteractHoverText : MonoBehaviour
{
    PlayerMotor motor;
    void Awake()
    {
        motor = gameObject.GetComponent<PlayerMotor>();
        Debug.Log(motor.GetInstanceID());
        Debug.Log("interacthovertext.cs is awake");
        if (motor != null)
        {
            motor.OnInteractableObjectHover += ShowHoverText;
            Debug.Log("event subscribed!");
        }
        else
        {
            Debug.Log("motor is null");
        }
    }

    void OnDisable()
    {
        motor.OnInteractableObjectHover -= ShowHoverText;
    }
    public void ShowHoverText()
    {
        Debug.Log("hover Event Triggered");
    }
}


