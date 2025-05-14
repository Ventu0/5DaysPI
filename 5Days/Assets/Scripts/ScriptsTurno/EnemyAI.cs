using UnityEngine;
using System.Collections.Generic;
public class EnemyAI : MonoBehaviour
{ 
    [SerializeField] List<BasicAttack> ataques;
    BasePersonagem enemyCharacter;
    void Start()
    {
        enemyCharacter = GetComponent<BasePersonagem>();
    }

    void Update()
    {
        
    }
    public void Attack()
    {
        if (!enemyCharacter.turnEnded && TurnModeManager.instance.turno == Turnos.EnemyTurn)
        {
            int ataqueEscolhido = Random.Range(0, ataques.Count);
            BasicAttack ataque = ataques[ataqueEscolhido];
            ataque.ExecutarAtaque(TurnModeManager.instance.EncontrarAlvo(), ataque.attackEffect);
        }
    }
}
