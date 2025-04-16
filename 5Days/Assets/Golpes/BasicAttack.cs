using System.Collections;
using UnityEngine;

    [CreateAssetMenu(menuName = "Ataque/AtaqueBásico")]
    public class BasicAttack : Attack
    {
        [SerializeField] 
        public override void ExecutarAtaque(BasePersonagem alvo)
        {
        //if(animatorController != null)
        //{
        //      Animator playerAnimator = TurnModeManager.instance.PlayerAnimator();
        //      playerAnimator.runtimeAnimatorController = animatorController;

        //} 
            TurnModeManager.instance.QuemEstaAtacando().MovePlayerToPos(new Vector2(alvo.transform.position.x - 2, alvo.transform.position.y));
            alvo.TakeDamage(dano);
        }

}


