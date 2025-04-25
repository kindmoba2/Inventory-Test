using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler,IPointerEnterHandler,IPointerExitHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private InventoryItem currentItem;


    [SerializeField] Image icon;
    [SerializeField] Image woundedIcon;
    [SerializeField] Text countText;
    [SerializeField] Text statusText;

    Tweener tween;

    public static bool isDrag=false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Set(InventoryItem item)
    {
        currentItem = item;
        icon.sprite = item.Data.Icon;
        icon.enabled = true;
        countText.text = item.Count.ToString();

        statusText.text = item.Data.ItemName;

        if (item.Data.Type == ItemType.Animal && item.State.Equals(AnimalState.Wounded))
        {
            woundedIcon.gameObject.SetActive(true);
        }
        else
        {
            woundedIcon.gameObject.SetActive(false);
        }
    }

    public void Clear()
    {
        currentItem = null;
        icon.enabled = false;
        icon.sprite = null;
        countText.text = "";
        statusText.text = "";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;

        isDrag = true;

        // Добавляем Canvas и устанавливаем sortingOrder
        var canvas = gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 1;

        gameObject.AddComponent<GraphicRaycaster>();

        canvasGroup.blocksRaycasts = false;

        //this.transform.DOShakeScale(1f);
        tween = this.transform.DOPunchScale(Vector3.one * 0.25f, 1f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        isDrag = false;

        // Удаляем Canvas и GraphicRaycaster после перетаскивания
        Destroy(GetComponent<GraphicRaycaster>());
        Destroy(GetComponent<Canvas>());


        tween?.Kill();
        InventoryManager.Instance.OnInventoryChanged?.Invoke();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<InventorySlot>();

        if (dragged == null || dragged == this)
        {
            return;
        }

        tween?.Kill();
        DOVirtual.DelayedCall(.01f, () => { InventoryManager.Instance.SwapItems(this, dragged); });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDrag) return;//волочим предмет, не надо подсказку

        if (currentItem != null)
            ItemTooltip.Show(currentItem, transform.position + Vector3.up * 5);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltip.Hide();
    }

    public InventoryItem GetItem() => currentItem;
}
