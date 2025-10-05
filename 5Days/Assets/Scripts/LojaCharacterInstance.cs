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
    [HideInInspector] public Button button;
    public RectTransform rect; //pra nao ter que ficar pegando os componentes no meio do for
    Coroutine MoveRoutine;
    bool hasEndedChangeAlpha = false;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        graphicsToChangeColor = GetComponentsInChildren<Graphic>();
        button.onClick.AddListener(() => button.enabled = false); //desativa quando clica ppra nao repetir o animSequence
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
        hasEndedChangeAlpha = true;
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
        if (!hasEndedChangeAlpha) return;
        gameObject.SetActive(false);
        hasEndedChangeAlpha = false;
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
