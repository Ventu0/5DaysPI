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
    public IEnumerator Mexer(RectTransform nextPos, float duration)
    {
        float t = 0;
        RectTransform rect = GetComponent<RectTransform>();
        while (t < duration)
        {
            t += Time.deltaTime;
            rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, nextPos.anchoredPosition, t / duration);
            rect.localScale = Vector3.Lerp(rect.localScale, nextPos.localScale, t / duration);
            yield return null;
        }
    }
}
