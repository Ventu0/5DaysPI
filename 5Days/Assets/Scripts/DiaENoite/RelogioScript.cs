using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Cinemachine;

public class RelogioScript : MonoBehaviour
{
    [Header("Configurações do Relógio")]
    [SerializeField] GameObject relogio;
    [SerializeField] Image gradiente;
    [SerializeField] TextMeshProUGUI hourText;
    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] float gradienteInitialX;
    [SerializeField] float gradienteFinalX;

    [Header("Configurações de Tempo")]
    [SerializeField] int minutes;
    public int hours = 0;
    [SerializeField] int maxHours = 23;
    [SerializeField] int timeToReset = 1;

    [Header("Read-Only")]
    public int currentDay = 1;
    [SerializeField] int lastTriggeredHour = -1;
    [Space]
    [SerializeField] bool isCompleted = false;

    [SerializeField] string time;

    DiaENoite dayScript;
    void Start()
    {
        dayScript = GetComponent<DiaENoite>();
        DayNightSave dayNightSave = GetComponent<DayNightSave>();
        var savedTime = dayNightSave.HasSave();
        if (savedTime.has)
        {
            Load(savedTime.data);
            return;
        }

        UpdateTime();
        StartCoroutine(MoveGradient(dayScript.AcharValorRestante(dayScript.iniciarEmQualHora)));
        hours = dayScript.iniciarEmQualHora;
        dayText.text = "Dia " + currentDay;
    }
    void Load(TimeData timeData)
    {
        currentDay = timeData.currentDay;
        hours = timeData.currentHour;
        minutes = timeData.currentMinute;

        UpdateTime();
        dayText.text = "Dia " + currentDay;

        dayScript.isPaused = false;
        StartCoroutine(MoveGradient(dayScript.AcharValorRestante(hours)));
    }
    public void NextDay()
    {
        print("Next Day");
        currentDay += 1;
        currentDay = Mathf.Clamp(currentDay, 1, 5);
        dayText.text = "Dia " + currentDay;
        if (currentDay + 1 == 6)
        {
            StartCoroutine(LoseRoutine());
        }
    }
    [ContextMenu("Perder agora")]
    void Lose() => StartCoroutine(LoseRoutine());
    public IEnumerator LoseRoutine()
    {
        Player.instance.canMove = false;
        isCompleted = true;
        CameraController camController = CameraController.instance;
        CinemachineFramingTransposer transposer = camController.cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        StartCoroutine(ShakeEffect.instance.ShakeCam(transposer, transposer.m_TrackedObjectOffset, 5f, 0.5f));
        yield return new WaitForSeconds(4);
        FadeController.instance.FadeInForHowMuchTime(2, () => UnityEngine.SceneManagement.SceneManager.LoadScene("Lost"));
        yield return new WaitForSeconds(2.5f);
        DeleteSave.instance.Deletar();
    }
    public void AddTime()
    {
        if (isCompleted) return;
        minutes += 10;
        minutes = Mathf.Clamp(minutes, 0, 60);
        UpdateTime();
    }
    void UpdateTime()
    {
        if(minutes >= 60)
        {
            hours += 1;
            hours = Mathf.Clamp(hours, 0, maxHours);
            minutes = 0;
        }
        if (hours == dayScript.horarioDaNoite && lastTriggeredHour < dayScript.horarioDaNoite)
        {
            lastTriggeredHour = dayScript.horarioDaNoite;
            print("noite começou");
            if(!dayScript.isOnDarkPlace)
            StartCoroutine(dayScript.ChangeToNight(dayScript.AcharValorRestante(dayScript.horarioDaNoite)));    
        }
        if (hours == maxHours && lastTriggeredHour < maxHours)
        {
            lastTriggeredHour = 0;
            DiaENoite diaENoite = GetComponent<DiaENoite>();
            minutes = 0;
            hours = 0;
            StartCoroutine(MoveGradient());
            if(!dayScript.isOnDarkPlace)
            StartCoroutine(diaENoite.ChangeToDay());
            //diaENoite.ResetTime();
        }
        if(hours == timeToReset && lastTriggeredHour < timeToReset)
        {
            lastTriggeredHour = timeToReset;
            NextDay();
        }

        time = hours.ToString("D2") + ":" + minutes.ToString("D2");
        hourText.text = time;
    }
    void Desmaiar()
    {
        Player.instance.canMove = false;
    }
    public void ResetTime()
    {
        StopAllCoroutines();
        isCompleted = false;
        gradiente.rectTransform.anchoredPosition = new Vector2(gradienteInitialX, gradiente.rectTransform.anchoredPosition.y);
        gradiente.gameObject.SetActive(true);
        int iniciarEmQualHora = dayScript.iniciarEmQualHora;
        float valorRestante = dayScript.AcharValorRestante(iniciarEmQualHora);
        minutes = 0;
        hours = iniciarEmQualHora;
        UpdateTime();
        StartCoroutine(MoveGradient(valorRestante));
    }
    IEnumerator MoveGradient(float tempoInicial = 0)
    {
        RectTransform rectTransform = gradiente.GetComponent<RectTransform>();

        float duration = dayScript.tempoParaNoite * 60 - tempoInicial;  

        float initialProgress = tempoInicial / dayScript.tempoParaNoiteSegundos;
        float initialLerp = Mathf.Lerp(gradienteInitialX, gradienteFinalX, initialProgress);
        rectTransform.anchoredPosition = new Vector2(initialLerp, rectTransform.anchoredPosition.y);

        float iterador = 0;
        while (iterador < duration)
        {
            while (dayScript.isPaused) yield return null;
            iterador += Time.deltaTime;;
            float x = Mathf.Lerp(gradienteInitialX, gradienteFinalX, Mathf.Clamp01(initialProgress + iterador / dayScript.tempoParaNoiteSegundos));
            rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
            yield return null;
        }   
    }
    public void SetActive(bool active)
    {
        relogio.SetActive(active);
    }
    public int GetCurrentHour()
    {
        return hours;
    }
    public int GetCurrentMinute()
    {
        return minutes;
    }
}
