using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class LojaCharacterInstance : MonoBehaviour
{
    [SerializeField] Image displayImage;
    [SerializeField] PersonagensNaLoja personagem;
    [SerializeField] Graphic[] graphicsToChangeColor;
    [HideInInspector]public Button button;
    Coroutine MoveRoutine;
    private void Awake()
    {
        button = GetComponent<Button>();
        graphicsToChangeColor = GetComponentsInChildren<Graphic>();

    }
    void Start()
    {
        ColorBlock color = button.colors;
        button.onClick.AddListener(() => TrocarCorOnClick(color.pressedColor)); //fiz esse pra trocar a cor quando clicar, provavelmente vou fazer um efeito de grow in e grow out e mexer pra posição fixa
    }
    public void Setup(PersonagensNaLoja novoPersonagem)
    {
        displayImage.sprite = novoPersonagem.display;
        personagem = novoPersonagem;
    }
    public void Move(Vector3 originalPos, Vector3 originalScale, float duration, bool setActive)
    {
        gameObject.SetActive(false);
        if (!setActive)
            return;
        gameObject.SetActive(true);
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
    public IEnumerator FadeAlpha(float duration)
    {
        float t = 0;
        float newAlpha = 0;
        while(t < duration)
        {
            newAlpha = Mathf.Lerp(1, 0, t / duration);
            ChangeAlpha(newAlpha);
            t += Time.deltaTime;
            yield return null;
        }
        newAlpha = 0;   
        ChangeAlpha(newAlpha);
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
        gameObject.SetActive(false);
    }
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
    void TrocarCorOnClick(Color color)
    {
        for (int i = 0; i < graphicsToChangeColor.Length; i++)
        {
            graphicsToChangeColor[i].color = color;
        }
    }
    public Coroutine HasEndedCoroutine()
    {
        return MoveRoutine;
    }
}
