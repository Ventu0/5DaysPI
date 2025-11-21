using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Collections;
public class SkipCutscene : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject skipCutsceneMenu;
    [SerializeField] Image radialFill;
    [SerializeField] TMP_Text secondsText;

    [Header("Settings")]
    [SerializeField] float timeToSkip = 2f;
    [SerializeField] UnityEvent onCutsceneSkip;

    [Header("Read-Only")]
    [SerializeField] float elapsedTime = 0f;
    public bool finished = false;
    public bool canSkipCutscene;
    void Start()
    {
        skipCutsceneMenu.SetActive(false);
    }
    private void Update()
    {
        if (finished || !canSkipCutscene) return;

        if (Input.GetButtonDown("Fire1"))
        {
            skipCutsceneMenu.SetActive(true);
            StartCoroutine(DecreaseTime());
        }
        if (Input.GetButton("Fire1"))
        {
            elapsedTime += Time.deltaTime;
            secondsText.text = Mathf.Max(timeToSkip - elapsedTime, 0).ToString("F1") + "s";
        }
        if(Input.GetButtonUp("Fire1"))
        {
            skipCutsceneMenu.SetActive(false);
            StopAllCoroutines();
            radialFill.fillAmount = 0;
            elapsedTime = 0f;
            secondsText.text = timeToSkip.ToString("F1") + "s";
        }
        if(timeToSkip <= elapsedTime)
        {
            finished = true;
            onCutsceneSkip.Invoke();
            skipCutsceneMenu.SetActive(false);
        }
    }
    public void CompletedSkip()
    {
        finished = true;
    }
    IEnumerator DecreaseTime()
    {
        float iterador = 0;
        while (iterador < timeToSkip)
        {
            radialFill.fillAmount = Mathf.Clamp01(iterador / timeToSkip);
            iterador += Time.deltaTime;
            yield return null;
        }
    }
}
