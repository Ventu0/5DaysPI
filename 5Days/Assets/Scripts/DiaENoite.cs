using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class DiaENoite : MonoBehaviour
{
    public GameObject dontDestroyObject;
    [SerializeField] Light2D directionalLight;
    public float tempoParaNoite = 1;
    [HideInInspector] public float tempoParaNoiteSegundos;
    [Range(0, 1)]
    [SerializeField] float intensidadeNoite = 0.2f;
    [Tooltip("Tempo(em minutos) para a noite")]
    float time;

    public delegate void OnNightChange();
    public OnNightChange onNightStart;
    
    [SerializeField] RelogioScript relogioScript;
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
            Destroy(gameObject);
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
        print("Noite Iniciada");
        onNightStart?.Invoke();
    }
}