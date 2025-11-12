using System;
using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField] Animator fadeAnimator;
    [SerializeField] bool persistent = true;
    public static FadeController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (!persistent) return;
            DontDestroyOnLoad(transform.root);
        }
        else
        {
            StartCoroutine(DestroyCanvasNextFrame());
        }
    }
    private IEnumerator DestroyCanvasNextFrame()
    {
        yield return null;
        Destroy(transform.root);
    }
    void Start()
    {
        fadeAnimator.gameObject.SetActive(false);
    }
    [ContextMenu("FadeOut")]
    public void FadeOut()
    {
        StopAllCoroutines();
        fadeAnimator.gameObject.SetActive(true);
        StartCoroutine(FadeCoroutine(2));
    }
    
    public void FadeInForHowMuchTime(float time, Action onFadeInHalf = null)
    {
        StartCoroutine(FadeCoroutine(time, onFadeInHalf));
    }
    public void FadeInWithoutAction(float time)
    {
        StartCoroutine(FadeCoroutine(time));
    }
    IEnumerator FadeCoroutine(float time, Action onFadeInHalf = null)
    {
        string parameterName = "Stay";
        float halfTime = time / 2f;

        fadeAnimator.gameObject.SetActive(true);
        fadeAnimator.SetBool(parameterName, true);

        yield return new WaitForSecondsRealtime(halfTime);
        onFadeInHalf?.Invoke();
        fadeAnimator.SetBool(parameterName, false);

        yield return new WaitForSecondsRealtime(halfTime);

        fadeAnimator.gameObject.SetActive(false);
        
    }
}
