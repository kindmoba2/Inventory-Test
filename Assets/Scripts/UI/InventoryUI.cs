using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slotParent;

    [Header("число столбцов")]
    [SerializeField] private int columnCount = 4; // Фиксированное число столбцов
    [SerializeField] private GridLayoutGroup gridLayoutGroup;//сетка инвентаря, будем её морщить при изменении разрешения

    [Space]
    [SerializeField] private GameObject slotPrefab;

    private List<InventorySlot> slots = new();

    private int lastWidth;
    private int lastHeight;



    private IEnumerator Start()
    {
        InventoryManager.Instance.OnInventoryChanged += Refresh;
        Refresh();


        yield return null;

        lastWidth = Screen.width;
        lastHeight = Screen.height;
        OnResolutionChanged();
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

    private void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;
            //Debug.Log($"Разрешение изменилось: {lastWidth}x{lastHeight}");

            // Вызываем метод для обработки изменения
            OnResolutionChanged();
        }
    }

    private void OnResolutionChanged()
    {
        print($"new Resolution: {Screen.width} {Screen.height}");
        RectTransform rectTransform = (RectTransform)gridLayoutGroup.transform;
        float containerWidth = rectTransform.rect.width;


        float availableWidth = containerWidth - gridLayoutGroup.padding.left - gridLayoutGroup.padding.right;

        float cellWidth = (availableWidth - (gridLayoutGroup.spacing.x * (columnCount - 1))) / columnCount;

        float cellHeight = cellWidth * 0.7f;// gridLayoutGroup.cellSize.y; // 

        // Применяем новый размер
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);


        RectTransform TooltipTransform = (RectTransform)ItemTooltip.instance.transform;
        TooltipTransform.sizeDelta = new Vector2(cellWidth + cellWidth, cellHeight + cellHeight);
    }
}
