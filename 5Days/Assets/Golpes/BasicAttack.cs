using System.Collections;
using UnityEngine;

    [CreateAssetMenu(menuName = "Ataque/AtaqueBásico")]
    public class BasicAttack : Attack
    {
    [SerializeField] 
        public override void ExecutarAtaque(BasePersonagem alvo)
        {
            
            alvo.TakeDamage(dano);
        }

}


