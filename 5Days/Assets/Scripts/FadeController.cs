using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField] Animator fadeAnimator;
    public static FadeController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root);
        }
        else
        {
            Destroy(transform.root);
        }
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
        StartCoroutine(FadeCoroutine(2, "Stay"));
    }
    public void FadeInForHowMuchTime(float time)
    {
        StartCoroutine(FadeCoroutine(time, "Stay"));
    }
    IEnumerator FadeCoroutine(float time, string parameterName)
    {
        float halfTime = time / 2f;

        fadeAnimator.gameObject.SetActive(true);
        fadeAnimator.SetBool(parameterName, true);

        yield return new WaitForSecondsRealtime(halfTime);

        fadeAnimator.SetBool(parameterName, false);

        yield return new WaitForSecondsRealtime(halfTime);

        fadeAnimator.gameObject.SetActive(false);
    }
}
