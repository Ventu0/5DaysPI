using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Aliados : MonoBehaviour
{
    [SerializeField] public List<Attack> ataques;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float duration = 2f;
    public bool jaAtacou;
    Vector2 initialPos;
    void Awake()
    {
        initialPos = transform.position;
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void MovePlayerToPos(Vector2 newPos)
    {
        StartCoroutine(MovePlayer(newPos));
    }
    public IEnumerator MovePlayer(Vector2 newPos)
    {
        float iterador = 0;
        print("mexendo player");
        while (iterador < duration)
        {
            iterador = Mathf.Clamp01(iterador);
            float playerNewY = Mathf.Lerp(initialPos.y, newPos.y, iterador) + 0.5f * Mathf.Sin(Mathf.PI * iterador);
            float playerNewX = Mathf.Lerp(initialPos.x, newPos.x, iterador);
            transform.position = new Vector2(playerNewX, playerNewY);
            iterador += Time.deltaTime * duration;
            yield return null;
        }
        transform.position = newPos;
    }

}
