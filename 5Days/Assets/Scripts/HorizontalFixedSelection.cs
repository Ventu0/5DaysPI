using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalFixedSelection : MonoBehaviour
{
    [SerializeField] HorizontalLayoutGroup layoutGroup; //to usando Layout que é mais facil de organizar e mexer tipo slider
    [SerializeField] RectTransform[] items;
    [SerializeField] RectTransform selection;
    [SerializeField] Vector2 selectionClamp;
    [Header("Read-Only")]
    [SerializeField] int currentSelected;
    [SerializeField] int fieldToMove;
    void Start()
    {
        fieldToMove = -1;
    }
    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Horizontal"))
        {
            print("antes do horizontal");
            if(!CheckIfCanMoveSelection((int)horizontal))
            {
                print("não pode mover mais");
                return;
            }
            SelectOther(new Vector2(horizontal, 0));
            if (fieldToMove == 2 || fieldToMove == -2)
                Mexeu(new Vector2(horizontal, 0));
        }
    }
    void Mexeu(Vector2 direction)
    {
        direction = -direction; //inverte o vetor, pra ficar certinho
        for(int i = 0; i < items.Length; i++)
        {
            fieldToMove = (int)direction.x * -1;
            items[i].anchoredPosition += direction * 160;
        }
    }
    void SelectOther(Vector2 direction)
    {
        int horizontal = direction.x > 0 ? 1 : -1;
        currentSelected += horizontal;
        fieldToMove += horizontal;
        currentSelected = Mathf.Clamp(currentSelected, 0, items.Length - 1);
        StartCoroutine(MexerSelect(horizontal, 0.2f));
    }
    IEnumerator MexerSelect(int sentido, float duration)
    {
        float iterador = 0;
        Vector2 selectionPos = new Vector2(selection.anchoredPosition.x, selection.anchoredPosition.y);
        Vector2 newPos = selectionPos + new Vector2(sentido * 53
            , 0);
        while (iterador < duration)
        {
            selectionPos = Vector2.Lerp(selection.anchoredPosition, newPos, iterador / duration);
            selectionPos.x = Mathf.Clamp(selectionPos.x, selectionClamp.x, selectionClamp.y);
            selection.anchoredPosition = selectionPos;
            iterador += Time.deltaTime;
            
            yield return null;
        }
    }
    bool CheckIfCanMoveSelection(int direction)
    {
        if(currentSelected + direction < 0 || currentSelected + direction > items.Length - 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
