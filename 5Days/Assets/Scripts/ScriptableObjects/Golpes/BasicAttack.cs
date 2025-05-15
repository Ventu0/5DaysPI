using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
[CreateAssetMenu(menuName = "Ataque/AtaqueBásico")]
    public class BasicAttack : Attack
    {
        public override async void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite)
        {
        float duração = TurnModeManager.instance.QuemEstaAtacando().duration;
        BasePersonagem quemEstaAtacando = TurnModeManager.instance.QuemEstaAtacando();
        InteractButtonsController.instance.menu.SetActive(false);
        
        Debug.Log(quemEstaAtacando);
        quemEstaAtacando.MovePlayerToPos(new Vector2(alvo.transform.position.x, alvo.transform.position.y));
        await Task.Delay(Mathf.CeilToInt(duração) * 250);
        MovesVisualEffect.instance.AttackEffect(attackSprite, new Vector2(alvo.transform.position.x, alvo.transform.position.y), animatorController);
        alvo.TakeDamage(dano);
        }
}


