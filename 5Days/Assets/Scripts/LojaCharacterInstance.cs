using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class LojaCharacterInstance : MonoBehaviour
{
    [SerializeField] Image displayImage;
    public PersonagensNaLoja personagem;
    [SerializeField] Graphic[] graphicsToChangeColor;
    [HideInInspector] public Button button;
    public RectTransform rect; //pra nao ter que ficar pegando os componentes no meio do for
    Coroutine MoveRoutine;
    private void Awake()
    {
        displayImage = displayImage != null ? displayImage : GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        graphicsToChangeColor = GetComponentsInChildren<Graphic>(); //desativa quando clica ppra nao repetir o animSequence
    }
    public void Setup(PersonagensNaLoja novoPersonagem)
    {
        displayImage.sprite = novoPersonagem.display;
        personagem = novoPersonagem;
    }
    #region Movimentação
    public void Move(Vector3 originalPos, Vector3 originalScale, float duration, bool setActive)
    {
        gameObject.SetActive(setActive);
        if (!setActive) return;

        if (MoveRoutine != null)
            StopCoroutine(MoveRoutine);

        MoveRoutine = StartCoroutine(Mexer(originalPos, originalScale, duration));
    }
    
    public IEnumerator Mexer(Vector3 originalPos, Vector3 originalScale, float duration)
    {
        float t = 0;
        RectTransform rect = GetComponent<RectTransform>();
        while (t < duration)
        {
            t += Time.deltaTime;
            rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, originalPos, t / duration);
            rect.localScale = Vector3.Lerp(rect.localScale, originalScale, t / duration);
            yield return null;
        }
        MoveRoutine = null;
    }
    #endregion
    
    public void StartChangeAlpha(LojaAnimSequence animSequence, float duration , bool invert = false)
    {
        gameObject.SetActive(true);
        StartCoroutine(animSequence.FadeAlpha(a => ChangeAlpha(a), duration, b => gameObject.SetActive(b), invert));
    }
    void ChangeAlpha(float alpha)
    {
        ColorBlock colorBlock = button.colors;
        Color thisColor = colorBlock.disabledColor;

        thisColor.a = alpha;

        colorBlock.disabledColor = thisColor;
        button.colors = colorBlock;
        for (int i = 0; i < graphicsToChangeColor.Length; i++)
        {
            graphicsToChangeColor[i].color = thisColor;
        }

    }
    //provavelmente vou ter que tirar ChangeAlpha e colocar a logica toda no TrocarCor, porque elas basicamente faz a mesma coisa
    public void TrocarCor(bool isSelected, Color newColor)
    {
        ColorBlock colors = button.colors;
        button.interactable = isSelected ? true : false;

        if (isSelected)
            colors.selectedColor = newColor;
        else
            colors.disabledColor = newColor;

        for (int i = 0; i < graphicsToChangeColor.Length; i++)
        {
            graphicsToChangeColor[i].color = newColor;
        }
    }
    public Coroutine HasEndedCoroutine()
    {
        return MoveRoutine;
    }
}
