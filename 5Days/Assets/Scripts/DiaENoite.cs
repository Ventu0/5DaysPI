using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class DiaENoite : MonoBehaviour
{
    [SerializeField] Light2D directionalLight;
    [SerializeField] float tempoParaNoite = 1;
    [Tooltip("Tempo(em minutos) para a noite")]
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
            directionalLight.intensity = Mathf.Lerp(1, 0, iterador / duration);
            yield return null;
        }
    }
}
