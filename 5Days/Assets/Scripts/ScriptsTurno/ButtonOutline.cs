using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] GameObject outlineObject;
    [SerializeField] Selectable selectable;

    [Header("Attack Buttons Only")]
    [SerializeField] bool isAnAttackButton = false;
    [SerializeField] int buttonID;

    void Awake()
    {
        outlineObject.SetActive(false);
    }
    #region Eventos
    public void OnPointerEnter(PointerEventData eventData)
    {
        outlineObject.SetActive(true);
        OpenDescriptionMenu();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outlineObject.SetActive(false);
        CloseDescriptionMenu();
    }

    public void OnSelect(BaseEventData eventData)
    {
        outlineObject.SetActive(true);
        OpenDescriptionMenu();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        outlineObject.SetActive(false);
        CloseDescriptionMenu();
    }
    void OnDisable()
    {
        outlineObject.SetActive(false);
    }
    #endregion
    void OpenDescriptionMenu()
    {
        if (isAnAttackButton)
        {
            selectable.AbrirMenu(buttonID);
        }
    }
    void CloseDescriptionMenu()
    {
        if (isAnAttackButton)
        {
            selectable.Close();
        }
    }
}
