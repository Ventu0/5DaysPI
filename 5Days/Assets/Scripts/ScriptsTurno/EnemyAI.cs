using UnityEngine;
using System.Collections.Generic;
public class EnemyAI : MonoBehaviour
{ 
    public List<Attack> ataques;
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
            Attack ataque = ataques[ataqueEscolhido];
            ataque.ExecutarAtaque(TurnModeManager.instance.EncontrarAlvo(), ataque.attackEffect);
            print("estou atacando agora");
        }
    }
}
