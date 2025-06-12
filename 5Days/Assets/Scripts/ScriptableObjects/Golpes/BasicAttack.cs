using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
[CreateAssetMenu(menuName = "Ataque/AtaqueBásico")]
    public class BasicAttack : Attack
    {
        TurnModeManager turnModeManager;
        public override async void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite)
        {
        float duração = TurnModeManager.instance.QuemEstaAtacando().duration;
        Vector2 alvoPos = new Vector2(alvo.transform.position.x, alvo.transform.position.y + 0.5f);
        BasePersonagem quemEstaAtacando = TurnModeManager.instance.QuemEstaAtacando();

        InteractButtonsController.instance.menu.SetActive(false);
        CharacterMovement.instance.Move(quemEstaAtacando, alvoPos, quemEstaAtacando.duration, quemEstaAtacando.shadow);
        

        await Task.Delay(Mathf.CeilToInt(duração) * 250);

        if (efeitoSecundario != null && alvo.efeitoAtivo == null) efeitoSecundario.ApplyEffect(alvo); //se tiver efeito secundario, ativar
        SFX.instance.PlaySFX(soundEffect, 1f);// tocar som do ataque
        if (ataqueEmArea)
        {
            
        }
        MovesVisualEffect.instance.AttackEffect(attackSprite, alvoPos, animation, true);
        alvo.TakeDamage(dano, shakeCamera);
        }
}