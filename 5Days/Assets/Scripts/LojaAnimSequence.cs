using UnityEngine;
using System.Collections;
using System;
public class LojaAnimSequence : MonoBehaviour
{
    [SerializeField] float finalX; 
    void Start()
    {
        
    }
    public void StartSequence(RectTransform selectedCharacter)
    {
        StartCoroutine(Sequence(selectedCharacter));
    }
    IEnumerator Sequence(RectTransform selectedCharacter)
    {
        Vector2 finalScale = selectedCharacter.localScale * 1.15f;
        Vector2 finalPos = new Vector2(selectedCharacter.anchoredPosition.x + finalX, 0);

        yield return StartCoroutine( //yield return StartCoroutine espera a outra Corotina terminar pra continuar, tipo uma sequencia
            Vector2LerpTween(selectedCharacter.localScale, finalScale, 0.25f, v => selectedCharacter.localScale = v, true));

        yield return StartCoroutine(
            Vector2LerpTween(selectedCharacter.anchoredPosition, finalPos, 0.5f, v => selectedCharacter.anchoredPosition = v));
    }
    IEnumerator Vector2LerpTween(Vector2 start, Vector2 end, float duration, Action<Vector2> valueToTween, bool InOut = false)
    {
        float completeDuration = InOut ? duration / 2 : duration;
        float t = 0;
        Vector2 value;
        while (t < completeDuration)
        {
            value = Vector2.Lerp(start, end, t / duration);
            t += Time.deltaTime;
            valueToTween?.Invoke(value);
            yield return null;
        }
        valueToTween?.Invoke(end);

        if (InOut) 
        yield return StartCoroutine(Vector2LerpTween(end, start, duration, valueToTween));
    }
}
