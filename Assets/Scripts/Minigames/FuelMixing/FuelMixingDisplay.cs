using Unity.Netcode;
using UnityEngine;

public class FuelMixingDisplay : NetworkBehaviour
{
    private void Start()
    {
        FuelMixing.OnFuelMixStart += PrintComboClientRpc;
    }

    public void PrintCombo(int[] combo)
    {
        PrintComboClientRpc(combo);
    }

    [ClientRpc]
    public void PrintComboClientRpc(int[] combo)
    {
        for (int i = 0; i < combo.Length; i++)
        {
            Debug.Log($"Item {i}: {combo[i]}");
        }
    }
}
