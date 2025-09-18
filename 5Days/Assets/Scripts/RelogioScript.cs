using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class RelogioScript : MonoBehaviour
{
    [Header("Configurações do Relógio")]
    [SerializeField] Image gradiente;
    [SerializeField] TextMeshProUGUI hourText;
    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] float gradienteInitialX;
    [SerializeField] float gradienteFinalX;

    [Header("Configurações de Tempo")]
    [SerializeField] string time;
    [SerializeField] int currentDay = 1;
    [SerializeField] int minutes;
    [HideInInspector] public int hours = 0;
    [SerializeField] int maxHours = 23;
    [SerializeField] bool isCompleted = false;
    DiaENoite dayScript;
    void Start()
    {
        dayScript = GetComponent<DiaENoite>();
        UpdateTime();
        StartCoroutine(MoveGradient(dayScript.AcharValorRestante(dayScript.iniciarEmQualHora)));
        hours = dayScript.iniciarEmQualHora;
        dayText.text = "Dia " + currentDay;
    }
    void Update()
    {
        
    }
    public void NextDay()
    {
        currentDay += 1;
        currentDay = Mathf.Clamp(currentDay, 1, 5);
        dayText.text = "Dia " + currentDay;
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
        if (hours == dayScript.horarioDaNoite)
        {
            StartCoroutine(dayScript.ChangeToNight(dayScript.AcharValorRestante(dayScript.horarioDaNoite)));
        }
        if (hours == maxHours)
        {
            DiaENoite diaENoite = GetComponent<DiaENoite>();
            minutes = 0;
            hours = 0;
            StartCoroutine(MoveGradient());
            StartCoroutine(diaENoite.ChangeToDay());
            //diaENoite.ResetTime();
        }

        time = hours.ToString("D2") + ":" + minutes.ToString("D2");
        hourText.text = time;
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
        StartCoroutine(MoveGradient(valorRestante));
    }
    IEnumerator MoveGradient(float tempoInicial = 0)
    {
        float duration = dayScript.tempoParaNoite * 60 - tempoInicial;
        print("Tempo inicial" + tempoInicial);
        float iterador = 0;
        RectTransform rectTransform = gradiente.GetComponent<RectTransform>();
        while (iterador < duration)
        {
            while (dayScript.isPaused) yield return null;
            iterador += Time.deltaTime;
            float x = Mathf.Lerp(gradienteInitialX, gradienteFinalX, iterador / duration);
            rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
            yield return null;
        }
        print("Relogio Completo");
    }
}
