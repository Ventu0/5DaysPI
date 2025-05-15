using UnityEngine;
using System.Collections;
public class ShakeEffect : MonoBehaviour
{
    public static ShakeEffect instance;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }
    public IEnumerator Shake(GameObject objectToShake, float duration, float strength)
    {
        Vector3 initialPos = objectToShake.transform.position;
        float iterador = 0;
        while (iterador < duration)
        {
            iterador += Time.deltaTime;
            float x = Random.Range(-1, 1) * strength;
            float y = Random.Range(-1, 1) * strength;
            objectToShake.transform.position = initialPos + new Vector3(x, y, 0);
            yield return null;
        }
        objectToShake.transform.position = initialPos;
    }


}
