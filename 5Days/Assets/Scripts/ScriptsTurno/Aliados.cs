using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Aliados : MonoBehaviour
{
    [SerializeField] public List<Attack> ataques;
    [SerializeField] RuntimeAnimatorController animatorController;
    [SerializeField] float duration = 2f;
    public bool jaAtacou;
    void Awake()
    {

    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    //public void MovePlayerToPos(Vector2 newPos)
    //{
    //    StartCoroutine(MovePlayer(newPos));
    //}
    ////public IEnumerator MovePlayer(Vector2 newPos)
    ////{
    ////    float iterador = 0;
    ////    while (iterador < duration)
    ////    {
    ////        float playerNewY = Mathf.Lerp(transform.position.y, newPos.y, curve.Evaluate(iterador));
    ////        float playerNewX = Mathf.Lerp(transform.position.x, newPos.x, iterador);
    ////        transform.position = new Vector2(playerNewX, playerNewY);
    ////        //transform.position = Vector3.Lerp(transform.position, new Vector2(newPos.x, playerNewY), iterador);
    ////        iterador += Time.deltaTime / duration;
    ////        yield return null;
    ////    }
    ////}

}
