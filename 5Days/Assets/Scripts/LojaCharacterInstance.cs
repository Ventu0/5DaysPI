using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(Button))]
public class LojaCharacterInstance : MonoBehaviour
{
    [SerializeField] Image displayImage;
    public PersonagensNaLoja personagem;
    [SerializeField] Graphic[] graphicsToChangeColor;
    [HideInInspector] public Button button;

    [Header("Read-Only")]
    public RectTransform rect; //pra nao ter que ficar pegando os componentes no meio do for
    [Space]
    [SerializeField] GameObject unlockedText;
    public bool unlocked;
    [Space]
    [SerializeField] Color disabledColor = new Color(51, 51, 50);
    Coroutine MoveRoutine;
    private void Awake()
    {
        displayImage = displayImage != null ? displayImage : GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        graphicsToChangeColor = GetComponentsInChildren<Graphic>(); //desativa quando clica ppra nao repetir o animSequence
        unlockedText.SetActive(false);
    }
    public void Setup(PersonagensNaLoja novoPersonagem, bool unlocked = false)
    {
        displayImage.sprite = novoPersonagem.display;
        personagem = novoPersonagem;
        if (unlocked) Comprado(false);
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
        if (unlocked) return; //evita trocar a cor
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
    public void Comprado(bool descelecionar = true)
    {
        print("ja fui desbloqueado");
        TrocarCor(false, disabledColor);
        unlocked = true;
        unlockedText.SetActive(true);
        if(descelecionar)
        LojaCharacterSelection.instance.DeselectCurrent();
    }
    public Coroutine HasEndedCoroutine()
    {
        return MoveRoutine;
    }
}
