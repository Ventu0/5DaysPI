using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBobing : MonoBehaviour
{
    [SerializeField] Light2D luz;//Referência para a luz que será mexida
    [SerializeField] float maxLightRadius = 1.5f; // Raio máximo da luz
    [SerializeField] float duration = 5f;
    void Start()
    {
        StartCoroutine(MexerRoutine());
    }
    void Update()
    {
        
    }
    IEnumerator MexerRoutine()
    {
        float iterador = 0;
        while (true)
        {
            while (iterador < duration)
            {
                iterador += Time.deltaTime;
                luz.pointLightOuterRadius = Mathf.Lerp(maxLightRadius, 1, iterador / duration);
                yield return null;
            }

            iterador = 0;
            yield return null;
            while (iterador < duration)
            {
                iterador += Time.deltaTime;
                luz.pointLightOuterRadius = Mathf.Lerp(1, maxLightRadius, iterador / duration);
                yield return null;
            }
            iterador = 0;
            yield return null;
        }
    }
}
