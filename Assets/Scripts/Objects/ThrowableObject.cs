using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

abstract public class ThrowableObject : NetworkBehaviour
{
    [SerializeField] private string hoverText;

    private NetworkVariable<bool> owned = new(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    private Rigidbody rb;
    private Collider col;
    private Transform handTransform;

    private void Start()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        gameObject.tag = "Throwable Object";
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        owned.OnValueChanged += (oldValue, newValue) => UpdateObjectState(newValue);
        UpdateObjectState(owned.Value);
    }

    private void Update()
    {
        if (IsOwner && owned.Value && handTransform != null)
        {
            transform.SetPositionAndRotation(handTransform.position, handTransform.rotation);
            ForceNetworkTransformSyncServerRpc(transform.position, transform.rotation);
        }

        if (IsOwner && (rb.linearVelocity != Vector3.zero || rb.angularVelocity != Vector3.zero))
        {
            ForceNetworkTransformSyncServerRpc(transform.position, transform.rotation);
        }
    }

    public string GetText() => hoverText;
    public bool GetOwned() => owned.Value;

    [ServerRpc]
    private void ForceNetworkTransformSyncServerRpc(Vector3 position, Quaternion rotation)
    {
        ForceNetworkTransformSyncClientRpc(position, rotation);
    }

    [ClientRpc]
    private void ForceNetworkTransformSyncClientRpc(Vector3 position, Quaternion rotation)
    {
        if (!IsOwner)
            transform.SetPositionAndRotation(position, rotation);
    }

    public void PickUp(GameObject player)
    {
        if (owned.Value) return;
        handTransform = player.GetComponent<PlayerLook>().hand.transform;
        RequestPickupServerRpc(player.GetComponent<NetworkObject>().OwnerClientId);
    }

    public void Drop()
    {
        if (!owned.Value) return;
        handTransform = null;
        RequestDropServerRpc();
    }

    public void Throw()
    {
        if (!owned.Value) return;
        handTransform = null;
        RequestThrowServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestPickupServerRpc(ulong playerId, ServerRpcParams rpcParams = default)
    {
        if (owned.Value) return;

        owned.Value = true;
        col.enabled = false;
        DisableColliderClientRpc();

        GetComponent<NetworkObject>().ChangeOwnership(playerId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestDropServerRpc()
    {
        if (!owned.Value) return;

        owned.Value = false;

        col.enabled = true;
        EnableColliderClientRpc();

        rb.isKinematic = false;
        GetComponent<NetworkObject>().RemoveOwnership();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestThrowServerRpc()
    {
        if (!owned.Value) return;

        owned.Value = false;

        col.enabled = true;
        EnableColliderClientRpc();

        rb.isKinematic = false;
        rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
        GetComponent<NetworkObject>().RemoveOwnership();
    }

    [ClientRpc]
    private void EnableColliderClientRpc()
    {
        col.enabled = true;
    }

    [ClientRpc]
    private void DisableColliderClientRpc()
    {
        col.enabled = false;
    }

    private void UpdateObjectState(bool isOwned)
    {
        GetComponent<Rigidbody>().isKinematic = isOwned;
    }
}
