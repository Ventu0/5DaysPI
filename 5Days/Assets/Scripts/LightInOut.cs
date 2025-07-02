using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LuzMexendo : MonoBehaviour
{
    [SerializeField] Light2D luz;//Referência para a luz que será mexida
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
                luz.pointLightOuterRadius = Mathf.Lerp(1.5f, 1, iterador / duration);
                yield return null;
            }

            iterador = 0;
            yield return null;
            while (iterador < duration)
            {
                iterador += Time.deltaTime;
                luz.pointLightOuterRadius = Mathf.Lerp(1, 1.5f, iterador / duration);
                yield return null;
            }
            iterador = 0;
            yield return null;
        }
    }
}
