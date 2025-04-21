using UnityEngine;
using System.Collections.Generic;
public class EnemyAI : CharacterStatus
{
    public bool canAttack;
    [SerializeField] List<BasicAttack> ataques;
    void Start()
    {
        canAttack = true;
    }

    void Update()
    {
        
    }
    public void Attack()
    {
        if (canAttack && TurnModeManager.instance.turno == Turnos.EnemyTurn)
        {
            int ataqueEscolhido = Random.Range(0, ataques.Count);
            BasicAttack ataque = ataques[ataqueEscolhido];
            ataque.ExecutarAtaque(TurnModeManager.instance.EncontrarAlvo());
            canAttack = false;
        }
    }
}
