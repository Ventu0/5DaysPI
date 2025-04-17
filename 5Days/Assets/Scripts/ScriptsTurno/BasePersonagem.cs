using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BasePersonagem : MonoBehaviour, IDamageable
{
    public int força;
    public int vida;
    public int defesa;
    public float duration;
    Vector2 initialPos;
    public bool jaAtacou;
    public int numeroDoPersonagem;
    void Start()
    {
        initialPos = transform.position;
    }
    public void TakeDamage(int damage)
    {
        if(vida > 0)
        {
            vida -= damage;
        }
        if(vida <= 0)
        {
            TurnModeManager.instance.aliadosPersonagens.Remove(this);
            TurnModeManager.instance.aliados.Remove(gameObject.GetComponent<Aliados>());
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
        Turnos turno = TurnModeManager.instance.turno;
        if (turno == Turnos.EnemyTurn)
        {
            newPos.x += 2;
        }else if(turno == Turnos.PlayerTurn)
        {
            newPos.x -= 2;
        }
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
        print("atacou");
        TurnModeManager.instance.CheckIfAllPlayersAttacked();
    }
}
