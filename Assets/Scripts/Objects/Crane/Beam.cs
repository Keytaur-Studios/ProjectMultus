using UnityEngine;

public class Beam : MonoBehaviour
{
    public Crane crane;

    public void OnTriggerEnter(Collider other)
    {
        //if (!other.gameObject.CompareTag("Crane Object"))
        //    return;

        //crane.target = other.gameObject;
        //crane.targetRB = other.gameObject.GetComponent<Rigidbody>();
    }

    public void OnTriggerExit(Collider other)
    {
        //    crane.target = null;
        //    crane.targetRB = null;
        //    crane.magnetMode = Crane.MagnetMode.detached;
    }
}
