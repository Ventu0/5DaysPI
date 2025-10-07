using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
public class LojaAnimSequence : MonoBehaviour
{
    [SerializeField] float finalX;
    [SerializeField] AnimationCurve selectedCurve;
    public UnityEvent onAnimEnd;
    public UnityEvent afterAnimReset;
    void Start()
    {

    }
    public void StartSequence(RectTransform selectedCharacter, bool returnPos = false)
    {
        StartCoroutine(Sequence(selectedCharacter, returnPos));
    }
    IEnumerator Sequence(RectTransform selectedCharacter, bool returnPos)
    {
        if (returnPos)
        {
            finalX *= -1;
        }
        Vector2 finalScale = selectedCharacter.localScale * 1.15f;
        Vector2 finalPos = new Vector2(selectedCharacter.anchoredPosition.x + finalX, 0);
        //yield return StartCoroutine espera a outra Corotina terminar pra continuar, tipo uma sequencia

        //yield return StartCoroutine( animação crescer e diminuir;
        //    Vector2LerpTween(selectedCharacter.localScale, finalScale, 0.25f, v => selectedCharacter.localScale = v, true));

        yield return StartCoroutine(
            Vector2LerpTween(selectedCharacter.anchoredPosition, finalPos, 0.5f, v => selectedCharacter.anchoredPosition = v));

        if (returnPos)
            finalX *= -1;

        onAnimEnd?.Invoke();
        
        ResetAnimEnd(returnPos);
    }
    public IEnumerator Vector2LerpTween(Vector2 start, Vector2 end, float duration, Action<Vector2> valueToTween, bool InOut = false)
    {
        float completeDuration = InOut ? duration / 2 : duration;
        float t = 0;
        Vector2 value;
        while (t < completeDuration)
        {
            
            value = Vector2.Lerp(start, end, selectedCurve.Evaluate(t / duration));
            t += Time.deltaTime;
            valueToTween?.Invoke(value);
            yield return null;
        }
        valueToTween?.Invoke(end);
        if (InOut) 
        yield return StartCoroutine(Vector2LerpTween(end, start, duration, valueToTween));

    }
    public void ResetAnimEnd(bool resetFirst)
    {
        onAnimEnd.RemoveAllListeners();

        afterAnimReset?.Invoke();

        afterAnimReset.RemoveAllListeners();
    }
    public IEnumerator FadeAlpha(Action<float> ChangeAlpha, float duration, Action<bool> onEnd, bool invert = false)
    {

        float t = 0;
        float newAlpha = 0;

        float startAlpha = invert ? 0 : 1;
        float endAlpha = invert ? 1 : 0;

        while (t < duration)
        {
            newAlpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            ChangeAlpha(newAlpha);
            t += Time.deltaTime;
            yield return null;
        }
       newAlpha = endAlpha;
       ChangeAlpha(newAlpha);

       onEnd?.Invoke(invert);
    }
}
