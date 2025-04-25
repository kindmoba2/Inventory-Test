using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;

    private List<InventorySlot> slots = new();

    private void Start()
    {
        InventoryManager.Instance.OnInventoryChanged += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);
        slots.Clear();

        foreach (var item in InventoryManager.Instance.Items)
        {
            var slotGO = Instantiate(slotPrefab, slotParent);
            var slot = slotGO.GetComponent<InventorySlot>();
            slot.Set(item);
            slots.Add(slot);
        }
    }
}
