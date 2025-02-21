using UnityEngine;

abstract public class ThrowableObject : MonoBehaviour
{
    [SerializeField]
    private string hoverText;
    [SerializeField]
    private GameObject owner;
    [SerializeField]
    private bool owned;

    private void Start()
    {
        gameObject.tag = "Throwable Object";
        owned = false;
    }

    // Attach object to player crosshair and stop other players
    // from interacting with it.
    public void Interact(GameObject player)
    {
        if (owned)
            return;

        owned = true;

        gameObject.transform.SetLocalPositionAndRotation(player.GetComponent<PlayerLook>().hand.transform.position, this.transform.rotation);
        gameObject.transform.SetParent(player.GetComponent<PlayerLook>().hand.transform);
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void StopInteract()
    {
        if (!owned) return;

        owned = false;

        gameObject.transform.SetParent(null);
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    public string GetText()
    {
        return hoverText;
    }

    public bool GetOwned()
    {
        return owned;
    }
}
