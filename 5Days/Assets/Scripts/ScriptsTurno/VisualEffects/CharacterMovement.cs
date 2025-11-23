using UnityEngine;
using System.Collections;
public class CharacterMovement : MonoBehaviour
{
    float standingStillDuration;
    Transform shadow;
    TurnModeManager turnModeManager;
    BasePersonagem character;
    [SerializeField] AnimationCurve timeCurve;
    [SerializeField] AnimationCurve curve; //curva até funciona, mas ela não é adaptativa, teria que mexer no script
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
    public void Move(BasePersonagem characterToMove, Vector2 newPos, float stillDuration, bool useLinearMovement = false, float xOffSet = 3, float yOffSet = 0)
    { 
        Turnos turno = turnModeManager.turno;
        if (turno == Turnos.EnemyTurn)
            newPos.x += xOffSet;
        else if (turno == Turnos.PlayerTurn)
            newPos.x -= xOffSet;

        shadow = characterToMove.shadow;
        character = characterToMove;  //coloca os parametros em variaveis, para facilitar+
        standingStillDuration = stillDuration;

        float duration = turnModeManager.QuemEstaAtacando().duration;
        float offset = character.characterStatus.YOffset;
        Vector2 playerNewPos = new Vector2(newPos.x, newPos.y + offset + yOffSet); //calcula a posição com base no Offset do personagem

        if (!useLinearMovement)
            StartCoroutine(AllyMove(characterToMove, playerNewPos, duration));
        else
            StartCoroutine(ShadowMove(characterToMove.transform, playerNewPos, duration, useLinearMovement));

        if (shadow != null)
        {
            Vector2 shadowPos = !useLinearMovement ? new Vector2(newPos.x, newPos.y - 0.45f) : new Vector2(newPos.x, newPos.y);
            StartCoroutine(ShadowMove(shadow, shadowPos, duration));
        }
    }

    IEnumerator AllyMove(BasePersonagem characterToMove, Vector2 newPos, float duration)
    {
            float iterador = 0;
            Transform characterTransform = characterToMove.transform;
            Vector2 initialPos = characterTransform.position;
            while (iterador < duration)
            {
                float playerNewY = Mathf.Lerp(initialPos.y, newPos.y, timeCurve.Evaluate(iterador)) + 0.5f * Mathf.Sin(Mathf.PI * timeCurve.Evaluate(Mathf.Clamp01(iterador)));
                
                float playerNewX = Mathf.Lerp(initialPos.x, newPos.x, timeCurve.Evaluate(iterador));
                characterTransform.position = new Vector2(playerNewX, playerNewY);
                iterador += Time.deltaTime * duration;
                yield return null;
            }

            yield return new WaitForSeconds(standingStillDuration);

            iterador = 0;
            while (iterador < duration)
            {
                float playerNewY = Mathf.Lerp(newPos.y, initialPos.y, timeCurve.Evaluate(iterador)) + 0.5f * Mathf.Sin(Mathf.PI * timeCurve.Evaluate(Mathf.Clamp01(iterador)));
                float playerNewX = Mathf.Lerp(newPos.x, initialPos.x, timeCurve.Evaluate(iterador));
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
            print("finalizando turno");
            character.EndTurn();
        }
    }
}
