using Unity.Netcode;
using UnityEngine;

public class Crane : NetworkBehaviour
{
    [Header("Pieces")]
    public GameObject chain;
    public GameObject magnet;
    public GameObject mech;

    [Header("Grabbing")]
    public LayerMask targetMask;
    public GameObject target;
    public Rigidbody targetRB;
    public MagnetMode magnetMode = MagnetMode.detached;

    public enum MagnetMode
    {
        detached, attached
    }

    [Header("Limits")]
    public float upL;
    public float downL;
    public float inL;
    public float outL;

    [Header("Speeds")]
    public float turnSpeed;
    public float vertSpeed;
    public float horizSpeed;

    [Header("Magnet")]
    public float pullForce;


    public void FixedUpdate()
    {
        MagnetSearchForTarget();

        if (magnetMode == MagnetMode.attached)
            Attach();
    }

    [ServerRpc (RequireOwnership = false)]
    public void DownServerRpc()
    {
        if (!IsHost)
            return;

        if (chain.transform.localScale.y >= downL)
            return;

        chain.transform.localScale += new Vector3(0, vertSpeed, 0);
        magnet.transform.localPosition = new Vector3(magnet.transform.localPosition.x, magnet.transform.localPosition.y - vertSpeed, magnet.transform.localPosition.z);
    }

    public void Down()
    {
        DownServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpServerRpc()
    {
        if (!IsHost)
            return;

        if (chain.transform.localScale.y <= upL)
            return;

        chain.transform.localScale += new Vector3(0, -vertSpeed, 0);
        magnet.transform.localPosition = new Vector3(magnet.transform.localPosition.x, magnet.transform.localPosition.y + vertSpeed, magnet.transform.localPosition.z);
    }

    [ServerRpc(RequireOwnership = false)]
    public void InServerRpc()
    {
        if (!IsHost)
            return;

        if (mech.transform.localPosition.z <= inL)
            return;

        mech.transform.localPosition = new Vector3(mech.transform.localPosition.x, mech.transform.localPosition.y, mech.transform.localPosition.z - horizSpeed);
    }

    [ServerRpc(RequireOwnership = false)]
    public void OutServerRpc()
    {
        if (!IsHost)
            return;

        if (mech.transform.localPosition.z >= outL)
            return;

        mech.transform.localPosition = new Vector3(mech.transform.localPosition.x, mech.transform.localPosition.y, mech.transform.localPosition.z + horizSpeed);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LeftServerRpc()
    {
        if (!IsHost)
            return;

        transform.Rotate(0, -turnSpeed, 0);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RightServerRpc()
    {
        if (!IsHost)
            return;

        transform.Rotate(0, turnSpeed, 0);
    }

    [ServerRpc(RequireOwnership = false)]
    public void MagnetToggleServerRpc()
    {
        if (!IsHost)
            return;

        if (target == null)
            return;

        switch (magnetMode)
        {
            case MagnetMode.attached:
                magnetMode = MagnetMode.detached;
                target.transform.SetParent(null);
                targetRB.useGravity = true;
                targetRB.isKinematic = false;
                break;
            case MagnetMode.detached:
                magnetMode = MagnetMode.attached;
                break;
        }
    }

    private void Attach()
    {
        //Vector3 direction = (magnet.transform.position - target.transform.position).normalized;
        //targetRB.AddForce(direction * pullForce * Time.deltaTime, ForceMode.Acceleration);

        Vector3 direction = (magnet.transform.position - target.transform.position).normalized;
        float distance = Vector3.Distance(magnet.transform.position, target.transform.position);
        float gravitationalPull = pullForce / (distance * distance); // Gravity decreases with distance

        if (target.transform.parent == null)
            targetRB.AddForce(direction * gravitationalPull);


        bool isHit = Physics.Raycast(magnet.transform.position, Vector3.down, out RaycastHit hitInfo, .4f, targetMask);

        if (!isHit)
        {
            target.transform.SetParent(null);
            targetRB.useGravity = true;
            targetRB.isKinematic = false;
            return;
        }

        if (!hitInfo.transform.gameObject.CompareTag("Crane Object"))
            return;

        target.transform.SetParent(magnet.transform);
        targetRB.useGravity = false;
        targetRB.isKinematic = true;


    }

    public void MagnetSearchForTarget()
    {
        bool isHit = Physics.Raycast(magnet.transform.position, Vector3.down, out RaycastHit hitInfo, 1f, targetMask);

        if (isHit)
            Debug.DrawLine(magnet.transform.position, hitInfo.point, Color.red);
        else
            Debug.DrawLine(magnet.transform.position, magnet.transform.position + (Vector3.down * .4f), Color.red);

        if (!isHit)
        {
            if (target != null)
                target.transform.SetParent(null);
            target = null;
            targetRB = null;
            magnetMode = MagnetMode.detached;
            return;
        }

        if (!hitInfo.transform.gameObject.CompareTag("Crane Object"))
            return;

        target = hitInfo.transform.gameObject;
        targetRB = target.GetComponent<Rigidbody>();
    }
}
