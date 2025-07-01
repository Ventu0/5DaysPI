using UnityEngine;
using System.Collections;
public class ShakeEffect : MonoBehaviour
{
    //[SerializeField] bool shakeOnStart = false;
    [SerializeField] GameObject shakeObject;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeStrength;
    public static ShakeEffect instance;
    void Awake()
    {
        if (instance == null)
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
    public void StartShake()
    {
        StartCoroutine(Shake(shakeObject, shakeDuration, shakeStrength));
    }
    public IEnumerator Shake(GameObject objectToShake, float duration, float strength, bool useLocalPosition = false)
    {
        Vector3 initialPos = objectToShake.transform.position;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            if(!useLocalPosition)
            objectToShake.transform.position = initialPos + new Vector3(x, y, 0f);
            else objectToShake.transform.localPosition = initialPos + new Vector3(x, y, 0f);

            yield return null;
        }

        objectToShake.transform.position = initialPos;

    }
}