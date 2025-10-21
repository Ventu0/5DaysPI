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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }
    public void FadeOut()
    {
        StopAllCoroutines();
        fadeAnimator.gameObject.SetActive(true);
        fadeAnimator.SetBool("End", true);
    }
    public void FadeInForHowMuchTime(float time)
    {
        StartCoroutine(FadeInCoroutine(time));
    }
    IEnumerator FadeInCoroutine(float time)
    {
        fadeAnimator.gameObject.SetActive(true);
        fadeAnimator.SetBool("Stay", true);
        yield return new WaitForSecondsRealtime(time);
        fadeAnimator.SetBool("Stay", false);
        fadeAnimator.gameObject.SetActive(false);
    }
}
