using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class RelogioScript : MonoBehaviour
{
    [Header("Configurações do Relógio")]
    [SerializeField] Image gradiente;
    [SerializeField] TextMeshProUGUI hourText;
    [SerializeField] float gradienteInitialX;
    [SerializeField] float gradienteFinalX;

    [Header("Configurações de Tempo")]
    [SerializeField] string time;
    [SerializeField] int minutes;
    [SerializeField] int hours = 6;
    [SerializeField] int maxHours = 23;
    [SerializeField] bool isCompleted = false;
    DiaENoite dayScript;
    void Start()
    {
        dayScript = GetComponent<DiaENoite>();
        UpdateTime();
        StartCoroutine(MoveGradient());
    }
    void Update()
    {
        
    }
    public void AddTime()
    {
        if (isCompleted) return;
        minutes += 10;
        minutes = Mathf.Clamp(minutes, 0, 60);
        UpdateTime();
    }
    public void ResetTime()
    {
        isCompleted = false;
        minutes = 0;
        hours = 6;
        gradiente.rectTransform.anchoredPosition = new Vector2(gradienteInitialX, gradiente.rectTransform.anchoredPosition.y);
    }
    void UpdateTime()
    {
        if(minutes >= 60)
        {
            hours = hours + minutes / 60;
            hours = Mathf.Clamp(hours, 6, maxHours);
            minutes = 0;
        }
        if (hours == maxHours)
        {
            print("Tempo completo");
            isCompleted = true;
        }

        time = hours.ToString("D2") + ":" + minutes.ToString("D2");
        hourText.text = time;
    }
    IEnumerator MoveGradient()
    {
        float duration = dayScript.tempoParaNoite * 60;
        float iterador = 0;
        RectTransform rectTransform = gradiente.GetComponent<RectTransform>();
        while (iterador < dayScript.tempoParaNoite * 60)
        {
            iterador += Time.deltaTime;
            float x = Mathf.Lerp(gradienteInitialX, gradienteFinalX, iterador / duration);
            rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
            yield return null;
        }
        print("Relogio Completo");
    }
}
