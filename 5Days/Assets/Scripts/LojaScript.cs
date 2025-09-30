using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LojaScript : MonoBehaviour
{
    [SerializeField] HorizontalLayoutGroup layoutGroup; //to usando Layout que é mais facil de organizar e mexer tipo slider
    [SerializeField] RectTransform[] items;
    [SerializeField] RectTransform selection;
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
            SelectOther(new Vector2(horizontal, 0));
            if(fieldToMove == 1)
            Mexeu(new Vector2(horizontal, 0));
            //layoutGroup.padding.left += Mathf.RoundToInt(horizontal * 157); //157 é o tamanho do item + espaçamento
        }
    }
    void Mexeu(Vector2 direction)
    {
        direction = -direction; //inverte o vetor, pra ficar certinho
        for(int i = 0; i < items.Length; i++)
        {
            fieldToMove = -1;
            items[i].anchoredPosition += direction * 160;
        }
    }
    void SelectOther(Vector2 direction)
    {
        int horizontal = direction.x > 0 ? 1 : -1;
        currentSelected += horizontal;
        fieldToMove += horizontal;
        currentSelected = Mathf.Clamp(currentSelected, 0, items.Length - 1);
        StartCoroutine(MexerSelect(items[currentSelected], 0.2f));
    }
    IEnumerator MexerSelect(RectTransform toPos, float duration)
    {
        float iterador = 0;
        while(iterador < duration)
        {
            selection.anchoredPosition = Vector2.Lerp(selection.anchoredPosition, toPos.anchoredPosition, iterador / duration);
            iterador += Time.deltaTime;
            yield return null;
        }
        selection.anchoredPosition = toPos.anchoredPosition;
    }
}
