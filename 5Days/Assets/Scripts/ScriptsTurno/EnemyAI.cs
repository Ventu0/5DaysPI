using UnityEngine;
using System.Collections.Generic;
public class EnemyAI : CharacterStatus
{
    [SerializeField] bool canAttack;
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
        if (canAttack)
        {
            int ataqueEscolhido = Random.Range(0, ataques.Count);
            BasicAttack ataque = ataques[ataqueEscolhido];
            //ataque.ExecutarAtaque();
            canAttack = false;
        }
    }
}
