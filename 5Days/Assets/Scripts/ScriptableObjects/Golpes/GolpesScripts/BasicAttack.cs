using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
[CreateAssetMenu(menuName = "Ataque/AtaqueBásico")]
    public class BasicAttack : Attack
    {
        [SerializeField] float stillDuration = 0.1f; //tempo parado na frente do inimigo
        TurnModeManager turnModeManager;
        public override async void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite)
        {
        turnModeManager = TurnModeManager.instance;
        float duração = turnModeManager.QuemEstaAtacando().duration;
        Vector2 alvoPos = new Vector2(alvo.transform.position.x, alvo.transform.position.y + 0.5f);
        BasePersonagem quemEstaAtacando = turnModeManager.QuemEstaAtacando();

        InteractButtonsController.instance.menu.SetActive(false);
        CharacterMovement.instance.Move(quemEstaAtacando, alvoPos, quemEstaAtacando.duration, quemEstaAtacando.shadow, stillDuration * quantidadesDeAtaque);
        

        await Task.Delay(Mathf.CeilToInt(duração) * 250); //tempo do pulo

        for(int i = 0; i < quantidadesDeAtaque; i++) //determina quantos ataques devem ocorrer
        {
            if (efeitoSecundario != null && alvo.efeitoAtivo == null) efeitoSecundario.ApplyEffect(alvo); //se tiver efeito secundario, ativar

            if (soundEffect != null) SFX.instance.PlaySFX(soundEffect, 1f);// tocar som do ataque

            if (ataqueEmArea)
            {
                Turnos turnos = turnModeManager.turno;
                List<BasePersonagem> alvos = turnModeManager.turno == Turnos.PlayerTurn ? turnModeManager.inimigosPersonagens : turnModeManager.aliadosPersonagens;
                for (int j = 0; j < alvos.Count; j++)
                {
                    MovesVisualEffect.instance.AttackEffect(attackSprite, alvos[i].transform.position, animation, animationPlayInFront);

                    if (efeitoSecundario != null && alvos[j].efeitoAtivo == null)
                        efeitoSecundario.ApplyEffect(alvos[j]);

                    if (alvo != alvos[j])
                        alvos[j].TakeDamage(danoOuCura / 2, shakeCamera);
                    else
                        alvo.TakeDamage(danoOuCura, shakeCamera);
                }
            } //se for, faz o ataque em area, se não, ataca normalmente
            else
            {
                MovesVisualEffect.instance.AttackEffect(attackSprite, alvoPos, animation, animationPlayInFront);

                alvo.TakeDamage(danoOuCura, shakeCamera);
            }
            await Task.Delay(Mathf.CeilToInt(effectsDuration) * 1000 / quantidadesDeAtaque);
        } 
    }
}