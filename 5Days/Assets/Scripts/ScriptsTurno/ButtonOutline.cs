using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] GameObject outlineObject;

    void Awake()
    {
        outlineObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        outlineObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outlineObject.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        outlineObject.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        outlineObject.SetActive(false);
    }
}
