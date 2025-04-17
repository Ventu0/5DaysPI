using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BasePersonagem : MonoBehaviour, IDamageable
{
    public int força;
    public int vida;
    public float duration;
    Vector2 initialPos;
    public bool jaAtacou;
    void Start()
    {
        initialPos = transform.position;
    }
    public void TakeDamage(int damage)
    {
        if(vida > 0)
        {
            vida -= damage;
        }else if(vida <= 0)
        {
            Destroy(gameObject);
        }

    }
    public void MovePlayerToPos(Vector2 newPos)
    {
        StartCoroutine(MovePlayer(newPos));
    }
    public IEnumerator MovePlayer(Vector2 newPos)
    {
        float iterador = 0;
        while (iterador < duration)
        {
            float playerNewY = Mathf.Lerp(initialPos.y, newPos.y, iterador) + 0.5f * Mathf.Sin(Mathf.PI * Mathf.Clamp01(iterador));
            float playerNewX = Mathf.Lerp(initialPos.x, newPos.x, iterador);
            transform.position = new Vector2(playerNewX, playerNewY);
            iterador += Time.deltaTime * duration;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        iterador = 0;
        while (iterador < duration)
        {
            float playerNewY = Mathf.Lerp(newPos.y, initialPos.y, iterador) + 0.5f * Mathf.Sin(Mathf.PI * Mathf.Clamp01(iterador));
            float playerNewX = Mathf.Lerp(newPos.x, initialPos.x, iterador);
            transform.position = new Vector2(playerNewX, playerNewY);
            iterador += Time.deltaTime * duration;
            yield return null;
        }
        jaAtacou = true;
        TurnModeManager.instance.CheckIfAllPlayersAttacked();
    }
}
