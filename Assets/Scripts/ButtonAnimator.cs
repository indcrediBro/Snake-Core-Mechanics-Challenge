using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private TMP_Text text;
    private Color originalColor;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySound("MouseClick");
    }

    private void OnEnable()
    {
        text = GetComponentInChildren<TMP_Text>();
        originalColor = text.color;
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        text.color = Color.red;
        transform.localScale *= 1.2f;
        AudioManager.Instance.PlaySound("MouseOver");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        text.color = originalColor;
        transform.localScale = Vector3.one;
    }
}
