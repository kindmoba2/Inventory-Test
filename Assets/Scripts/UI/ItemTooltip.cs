using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public Text nameText;
    public Text descriptionText;
    public CanvasGroup canvasGroup;

    public static ItemTooltip instance;

    //private Tween showTween;

    private void Start()
    {
        instance = this;
    }

    public static void Show(InventoryItem item, Vector3 position)
    {
        instance.nameText.text = item.Data.ItemName;
        instance.descriptionText.text = $"“ËÔ: {item.Data.Type}\nID: {item.Data.ID}";
        instance.transform.position = position;

        //showTween?.Kill();
        instance.canvasGroup.alpha = 0;
        instance.canvasGroup.DOFade(1f, 1f);
    }

    public static void Hide()
    {
        //showTween?.Kill();
        instance.canvasGroup.DOFade(0f, 1f);
    }
}
