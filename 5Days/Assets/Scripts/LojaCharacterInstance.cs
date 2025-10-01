using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LojaCharacterInstance : MonoBehaviour
{
    [SerializeField] Image displayImage;
    [SerializeField] PersonagensNaLoja personagem;
    public Vector2 originalScale;
    public Vector2 originalPos;
    bool isOnRoutine;

    void Start()
    {
        originalScale = GetComponent<RectTransform>().localScale;
    }
    public void Setup(PersonagensNaLoja novoPersonagem)
    {
        displayImage.sprite = novoPersonagem.display;
        personagem = novoPersonagem;
    }
    void Update()
    {
        
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
    }
}
