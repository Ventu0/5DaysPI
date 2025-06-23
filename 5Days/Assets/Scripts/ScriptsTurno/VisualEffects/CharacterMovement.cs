using UnityEngine;
using System.Collections;
public class CharacterMovement : MonoBehaviour
{
    float standingStillDuration;
    Transform shadow;
    TurnModeManager turnModeManager;
    BasePersonagem character;
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
    private void Start()
    {
        turnModeManager = TurnModeManager.instance;
    }
    public void Move(BasePersonagem characterToMove, Vector2 newPos, float stillDuration, bool useLinearMovement = false)
    { 
        Turnos turno = turnModeManager.turno;
        if (turno == Turnos.EnemyTurn)
            newPos.x += 3;
        else if (turno == Turnos.PlayerTurn)
            newPos.x -= 3;

        float duration = turnModeManager.QuemEstaAtacando().duration;
        shadow = characterToMove.shadow;
        character = characterToMove;
        standingStillDuration = stillDuration;

        if (!useLinearMovement)
            StartCoroutine(AllyMove(characterToMove, newPos, duration));
        else
            StartCoroutine(ShadowMove(characterToMove.transform, newPos, duration, useLinearMovement));
        if (shadow != null)
        {
            StartCoroutine(ShadowMove(shadow, new Vector2(newPos.x, newPos.y - 0.45f), duration));
        }
    }

    IEnumerator AllyMove(BasePersonagem characterToMove, Vector2 newPos, float duration)
    {
            float iterador = 0;
            Transform characterTransform = characterToMove.transform;
            Vector2 initialPos = characterTransform.position;
            while (iterador < duration)
            {
                float playerNewY = Mathf.Lerp(initialPos.y, newPos.y, iterador) + 0.5f * Mathf.Sin(Mathf.PI * Mathf.Clamp01(iterador));
                float playerNewX = Mathf.Lerp(initialPos.x, newPos.x, iterador);
                characterTransform.position = new Vector2(playerNewX, playerNewY);
                iterador += Time.deltaTime * duration;
                yield return null;
            }

            yield return new WaitForSeconds(standingStillDuration);

            iterador = 0;
            while (iterador < duration)
            {
                float playerNewY = Mathf.Lerp(newPos.y, initialPos.y, iterador) + 0.5f * Mathf.Sin(Mathf.PI * Mathf.Clamp01(iterador));
                float playerNewX = Mathf.Lerp(newPos.x, initialPos.x, iterador);
                characterTransform.position = new Vector2(playerNewX, playerNewY);
                iterador += Time.deltaTime * duration;
                yield return null;
            }
        character.EndTurn();
    } 
    IEnumerator ShadowMove(Transform shadow, Vector2 newPos, float duration, bool useLinearMovement = false)
    {
        float iterador = 0;
        Vector2 initialPos = shadow.position;
        if(!useLinearMovement)
        newPos.y -= 0.5f;
        while(iterador < duration)
        {
            iterador += Time.deltaTime * duration;
            shadow.position = Vector2.Lerp(initialPos, newPos, iterador);
            yield return null;
        }

        yield return new WaitForSeconds(standingStillDuration);

        iterador = 0;
        while(iterador < duration)
        {
            iterador += Time.deltaTime * duration;
            shadow.position = Vector2.Lerp(newPos, initialPos, iterador);
            yield return null;
        }
        if (useLinearMovement)
        {
            character.EndTurn();
        }
    }
}
