using UnityEngine;
using System.Collections;
public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement instance;
    
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

    public void Move(BasePersonagem characterToMove, Vector2 newPos, float duration, Transform shadow)
    {
        Turnos turno = TurnModeManager.instance.turno;
        if (turno == Turnos.EnemyTurn)
            newPos.x += 2;
        else if (turno == Turnos.PlayerTurn)
            newPos.x -= 2;

        StartCoroutine(AllyMove(characterToMove, newPos, duration));
        if(shadow != null)
        {
            StartCoroutine(ShadowMove(shadow, newPos, duration));
        }
    }

    IEnumerator AllyMove(BasePersonagem characterToMove, Vector2 newPos, float duration)
    {
            float iterador = 0;
            Transform characterTransform = characterToMove.transform;
            Vector2 initialPos = characterTransform.position;
            print("characterDuration = " + duration);
            while (iterador < duration)
            {
                float playerNewY = Mathf.Lerp(initialPos.y, newPos.y, iterador) + 0.5f * Mathf.Sin(Mathf.PI * Mathf.Clamp01(iterador));
                float playerNewX = Mathf.Lerp(initialPos.x, newPos.x, iterador);
                characterTransform.position = new Vector2(playerNewX, playerNewY);
                iterador += Time.deltaTime * duration;
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);

            iterador = 0;
            while (iterador < duration)
            {
                float playerNewY = Mathf.Lerp(newPos.y, initialPos.y, iterador) + 0.5f * Mathf.Sin(Mathf.PI * Mathf.Clamp01(iterador));
                float playerNewX = Mathf.Lerp(newPos.x, initialPos.x, iterador);
                characterTransform.position = new Vector2(playerNewX, playerNewY);
                iterador += Time.deltaTime * duration;
                yield return null;
            }
            characterToMove.turnEnded = true;
            TurnModeManager.instance.CheckIfAllCharactersAttacked();
    } 
    IEnumerator ShadowMove(Transform shadow, Vector2 newPos, float duration)
    {
        float iterador = 0;
        Vector2 initialPos = shadow.position;
        newPos.y -= 0.5f;
        while(iterador < duration)
        {
            iterador += Time.deltaTime * duration;
            shadow.position = Vector2.Lerp(initialPos, newPos, iterador);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        iterador = 0;
        while(iterador < duration)
        {
            iterador += Time.deltaTime * duration;
            shadow.position = Vector2.Lerp(newPos, initialPos, iterador);
            yield return null;
        }
    }
}
