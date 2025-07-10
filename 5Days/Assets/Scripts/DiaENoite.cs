using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class DiaENoite : MonoBehaviour
{
    [SerializeField] GameObject clockUI;
    public GameObject dontDestroyObject;
    public Light2D directionalLight;
    [SerializeField] Color corDaManha = new Color(218, 255, 254);
    [SerializeField] Color corDaTarde = new Color(218, 255, 254);
    public float tempoParaNoite = 1;
    [HideInInspector] public float tempoParaNoiteSegundos;
    [Range(0, 1)]
    [SerializeField] float intensidadeNoite = 0.2f;
    [Tooltip("Tempo(em minutos) para a noite")]
    float time;

    public delegate void OnNightChange();
    public OnNightChange onNightStart;
    
    RelogioScript relogioScript;
    public static DiaENoite instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(dontDestroyObject);
        }
        else
        {
            directionalLight.gameObject.SetActive(false);
            Destroy(dontDestroyObject);
        }
    }
    void Start()
    {
        relogioScript = GetComponent<RelogioScript>();
        tempoParaNoiteSegundos = tempoParaNoite * 60;
        StartCoroutine(ChangeToNight());

    }
    
    void Update()
    {
        time += Time.deltaTime;
        if(time >= tempoParaNoiteSegundos / 110) //verifica time usando tempoParaNoiteSegundos dividido por 113, pois o tempo entre 6 e 23 da 17, vezes 6 da 102
        {
            relogioScript.AddTime();
            time = 0;
        }
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
    IEnumerator ChangeToNight()
    {
        float iterador = 0;
        float duration = tempoParaNoiteSegundos; 
        while (iterador < duration)
        {
            iterador += Time.deltaTime;
            directionalLight.intensity = Mathf.Lerp(1, intensidadeNoite, iterador / duration);
            yield return null;
        }
        onNightStart?.Invoke();
    }
}