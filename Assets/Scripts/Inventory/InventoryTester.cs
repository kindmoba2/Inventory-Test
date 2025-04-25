using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] ItemData testItem;
    [SerializeField] Dropdown dropdown;

    private void Start()
    {
        var allItems = Resources.LoadAll<ItemData>("Items").ToList();


        //колдуем с дропдауном
        dropdown.ClearOptions();

        List<string> itemNames = allItems.Select(item => item.name).ToList();
        dropdown.AddOptions(itemNames);

        testItem= allItems[0];
        dropdown.onValueChanged.AddListener((index)=> { testItem = allItems[index]; });
    }




    public void AddTestItem()
    {
        InventoryManager.Instance.AddItem(testItem, testItem.Type == ItemType.Animal ? AnimalState.Healthy : AnimalState.Wounded);
    }

    public void RemoveTestItem()
    {
        InventoryManager.Instance.RemoveItem(testItem);
        //InventoryManager.Instance.RemoveItem(testItem, testItem.Type == ItemType.Animal ? AnimalState.Healthy : null);
    }

    public void ToggleTestItem()
    {
        InventoryManager.Instance.ToggleAnimalState(testItem);
    }

    public void OnClickDeleteAll()
    {
        PlayerPrefs.DeleteAll();
        InventoryManager.Instance.Items.Clear();
        InventoryManager.Instance.OnInventoryChanged.Invoke();
    }
}
