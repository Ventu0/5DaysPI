using System.Collections;
using UnityEngine;

public class LojaCharacterInstance : MonoBehaviour
{
    [SerializeField] int id;
    void Start()
    {
        
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
