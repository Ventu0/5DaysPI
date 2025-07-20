using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class SelectClickTarget : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] int personalID;
    SelectTarget selectTarget;

    void Start()
    {
        selectTarget = SelectTarget.instance;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        selectTarget.currentCharacterSelected = personalID;
        if (selectTarget.isSelecting)
        {
            selectTarget.CallMoveArrow(personalID);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(selectTarget.isSelecting)
        selectTarget.FinishSelect();
    }
}
