using UnityEngine;

abstract public class Resource : ThrowableObject
{
    [SerializeField]
    private string resourceName;
    [SerializeField]
    private int resourceID;

    public int GetResourceID()
    {
        return resourceID;
    }
}
