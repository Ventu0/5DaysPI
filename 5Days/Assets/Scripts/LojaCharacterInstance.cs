using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LojaCharacterInstance : MonoBehaviour
{
    [SerializeField] Image displayImage;
    [SerializeField] PersonagensNaLoja personagem;
    [SerializeField] Graphic[] graphicsToChangeColor;
    Button button;
    public Vector2 originalScale;
    public Vector2 originalPos;
    Coroutine MoveRoutine;
    void Start()
    {
        button = GetComponent<Button>();
        graphicsToChangeColor = GetComponentsInChildren<Graphic>();
        originalScale = GetComponent<RectTransform>().localScale;
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
    public void TrocarCor(bool isSelected, Color newColor)
    {
        ColorBlock colors = button.colors;
        if (isSelected)
        colors.selectedColor = newColor;
        else
        colors.disabledColor = newColor;
        for (int i = 0; i < graphicsToChangeColor.Length; i++)
        {
            graphicsToChangeColor[i].color = newColor;
        }
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
    public Coroutine HasEndedCoroutine()
    {
        return MoveRoutine;
    }
}
