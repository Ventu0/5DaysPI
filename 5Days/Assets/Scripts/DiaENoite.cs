using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEngine.SceneManagement;

public class DiaENoite : MonoBehaviour
{
    public GameObject clockUI;
    public GameObject dontDestroyObject;
    public Light2D directionalLight;

    [Header("Configurações do tempo")]
    public float tempoParaNoite = 1;
    [Tooltip("Tempo(em minutos) para a noite")]

    [HideInInspector] public float tempoParaNoiteSegundos;

    [Range(0, 1)]
    [SerializeField] float intensidadeNoite = 0.2f;

    [Range(0, 24)]
    public int iniciarEmQualHora = 0;

    [Range(0, 24)]
    public int horarioDaNoite = 18;

    [Header("Read-Only")]
    public bool isPaused;
    float time;

    public delegate void OnNightChange();
    public OnNightChange onNightStart;

    public delegate void OnDayBegin();
    public OnDayBegin onDayBegin;

    [HideInInspector] public RelogioScript relogioScript;

    public static DiaENoite instance;
    
    private void Awake()
    {
        directionalLight.gameObject.SetActive(false);
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(dontDestroyObject);
            directionalLight.gameObject.SetActive(true);
        }
        else
        {
            directionalLight.gameObject.SetActive(false);
            Destroy(dontDestroyObject);
        }
        relogioScript = GetComponent<RelogioScript>();
        tempoParaNoiteSegundos = tempoParaNoite * 60;
    }
    void Start()
    {
        
    }
    public float AcharValorRestante(float valorParaAcharPorcentagem)
    {
        float porcentagem = valorParaAcharPorcentagem / 24f * 100f; //acha a porcentagem do horario dentre as 24 horas
        float tempoInicial = (porcentagem / 100) * tempoParaNoiteSegundos; //acha o valor da porcentagem aplicado no tempoParaNoite
        return tempoInicial;
    }
    void Update()
    {
        time += Time.deltaTime;
        if(time >= tempoParaNoiteSegundos / 144 && !isPaused) //verifica time usando tempoParaNoiteSegundos dividido por 110, pois o tempo entre 6 e 23 da 17, vezes 6 da 102
        {
            relogioScript.AddTime();
            time = 0;
        }
    }
    public void PauseTime(bool pause = true)
    {
        isPaused = pause;
    }
    public void ResetTime()
    {
        clockUI.SetActive(true);
        time = 0;
        directionalLight.intensity = 1;
        relogioScript.ResetTime();
        StopAllCoroutines();
        StartCoroutine(ChangeToNight());
    }
    public void SetSpecificHour(float tempo) 
    {
      clockUI.SetActive(false);
      directionalLight.intensity = Mathf.Lerp(1, intensidadeNoite, tempo);
      StopAllCoroutines();
    }
    #region LerpsDeTempo
    public IEnumerator ChangeToNight(float tempoInicial = 0)
    {
        float iterador = 0;
        float tempoAtual = tempoParaNoiteSegundos - tempoInicial;
        float duration = tempoAtual;
        onNightStart?.Invoke();

        while (iterador < duration)
        {
            while(isPaused) yield return null;
            iterador += Time.deltaTime;
            directionalLight.intensity = Mathf.Lerp(1, intensidadeNoite, iterador / duration);
            yield return null;
        }
        print("Noite Iniciada");
    }
    public IEnumerator ChangeToDay()
    {
        float iterador = 0;
        float duration = tempoParaNoiteSegundos / 4;
        onDayBegin?.Invoke();

        while (iterador < duration)
        {
            while (isPaused) yield return null;
            iterador += Time.deltaTime;
            directionalLight.intensity = Mathf.Lerp(intensidadeNoite, 1, iterador / duration);
            yield return null;
        }
        print("Dia Iniciado");
    }
    #endregion
}