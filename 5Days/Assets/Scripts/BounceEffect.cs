using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BounceEffect : MonoBehaviour
{
    public bool isOnRoutine = false;
    public static BounceEffect instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public IEnumerator Bounce(Transform tranform, float bobbingDuration, float bounceQuantity, Action onEnd = null)
    {
        if (isOnRoutine) yield break;
        isOnRoutine = true;
        float iterador = 0;
        Vector2 newPos = new Vector2(tranform.position.x, tranform.position.y + bounceQuantity);
        while (iterador < bobbingDuration)
        {
            iterador += Time.deltaTime / bobbingDuration;
            tranform.position = Vector2.Lerp(tranform.position, newPos, iterador);
            yield return null;
        }

        yield return null;

        iterador = 0;
        newPos = new Vector2(tranform.position.x, tranform.position.y - bounceQuantity);
        while (iterador < bobbingDuration)
        {
            iterador += Time.deltaTime / bobbingDuration;
            tranform.position = Vector2.Lerp(tranform.position, newPos, iterador);
            yield return null;
        }
        tranform.position = newPos;
        yield return new WaitForSeconds(0.5f);
        onEnd?.Invoke();
        isOnRoutine = false;
    }
}
