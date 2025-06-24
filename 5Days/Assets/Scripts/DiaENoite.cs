using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class DiaENoite : MonoBehaviour
{
    [SerializeField] Light2D directionalLight;
    [SerializeField] float tempoParaNoite = 1;
    [Range(0, 1)]
    [SerializeField] float intensidadeNoite = 0.2f;
    [Tooltip("Tempo(em minutos) para a noite")]
    public delegate void OnNightChange();
    public OnNightChange onNightStart;
    public static DiaENoite instance;
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
        StartCoroutine(ChangeToNight());
    }

    
    void Update()
    {
        
    }
    IEnumerator ChangeToNight()
    {
        float iterador = 0;
        float duration = tempoParaNoite * 60; // Convertendo minutos para segundos
        while (iterador < duration)
        {
            iterador += Time.deltaTime;
            directionalLight.intensity = Mathf.Lerp(1, intensidadeNoite, iterador / duration);
            yield return null;
        }
        onNightStart?.Invoke();
    }
}
