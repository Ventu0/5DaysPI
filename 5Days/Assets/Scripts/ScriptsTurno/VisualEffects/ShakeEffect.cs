using UnityEngine;
using System.Collections;
using Cinemachine;
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
    public void ShakeCutscene()
    {
        StartCoroutine(Shake(shakeObject, shakeObject.transform.position, shakeDuration, shakeStrength));
    }
    public IEnumerator Shake(GameObject objectToShake, Vector3 originalPos, float duration, float strength, bool useLocalPosition = false)
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
            print("object to shake pos: " + objectToShake.transform.position);
            yield return null;
        }

        objectToShake.transform.position = originalPos;
        print("brutal, ja acabei");
    }
    public IEnumerator ShakeCam(CinemachineFramingTransposer transposer, Vector3 originalOffset, float duration, float strength)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            transposer.m_TrackedObjectOffset = originalOffset + new Vector3(x, y, 0f);

            yield return null;
        }
        transposer.m_TrackedObjectOffset = originalOffset;
    }
}