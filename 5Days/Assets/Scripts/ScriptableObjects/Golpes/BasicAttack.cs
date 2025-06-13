using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
[CreateAssetMenu(menuName = "Ataque/AtaqueBásico")]
    public class BasicAttack : Attack
    {
        TurnModeManager turnModeManager;
        public override async void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite)
        {
        turnModeManager = TurnModeManager.instance;
        float duração = turnModeManager.QuemEstaAtacando().duration;
        Vector2 alvoPos = new Vector2(alvo.transform.position.x, alvo.transform.position.y + 0.5f);
        BasePersonagem quemEstaAtacando = turnModeManager.QuemEstaAtacando();

        InteractButtonsController.instance.menu.SetActive(false);
        CharacterMovement.instance.Move(quemEstaAtacando, alvoPos, quemEstaAtacando.duration, quemEstaAtacando.shadow);
        

        await Task.Delay(Mathf.CeilToInt(duração) * 250);

        if (efeitoSecundario != null && alvo.efeitoAtivo == null) efeitoSecundario.ApplyEffect(alvo); //se tiver efeito secundario, ativar
        if(soundEffect != null) SFX.instance.PlaySFX(soundEffect, 1f);// tocar som do ataque
        if (ataqueEmArea)
        {
            Turnos turnos = turnModeManager.turno;
            List<BasePersonagem> alvos = turnModeManager.turno == Turnos.PlayerTurn ? turnModeManager.inimigosPersonagens : turnModeManager.aliadosPersonagens;
            for (int i = 0; i < alvos.Count; i++)
            {
                MovesVisualEffect.instance.AttackEffect(attackSprite, alvoPos, animation, true);
                if (efeitoSecundario != null && alvos[i].efeitoAtivo == null) efeitoSecundario.ApplyEffect(alvos[i]);
                alvos[i].TakeDamage(dano, shakeCamera);
            }
        }
        else
        {
            MovesVisualEffect.instance.AttackEffect(attackSprite, alvoPos, animation, animationPlayInFront);

            alvo.TakeDamage(dano, shakeCamera);
        } 
    }
}