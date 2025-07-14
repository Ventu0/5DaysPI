using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
[CreateAssetMenu(menuName = "Ataques/AtaqueBásico")]

    public class BasicAttack : Attack
    {
        [Header("Configurações de ataque básico")]
        public int quantidadesDeAtaque = 1;

        [SerializeField] float stillDuration = 0.1f; //tempo parado na frente do inimigo
        TurnModeManager turnModeManager;
        public override async void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite) //tipo de alvo: O Inimigo
        {
        if(ataqueUmaVezSó)
            if(oneTime) return; //se for ataque uma vez só e já tiver sido usado, não faz nada

        turnModeManager = TurnModeManager.instance;

        float duração = turnModeManager.QuemEstaAtacando().duration;
        Vector2 alvoPos = new Vector2(alvo.transform.position.x, alvo.transform.position.y + 0.5f);
        BasePersonagem quemEstaAtacando = turnModeManager.QuemEstaAtacando();
        Animator characterAnimator = quemEstaAtacando.GetComponent<Animator>();

        InteractButtonsController.instance.menu.SetActive(false);
        CharacterMovement.instance.Move(quemEstaAtacando, alvoPos, VisualEffectDuration * stillDuration * quantidadesDeAtaque, usarMovimentoLinear);
        currentPP = Mathf.Abs(currentPP - 1);
        if(ataqueUmaVezSó) oneTime = true;

        await Task.Delay(Mathf.CeilToInt(duração) * 250); //tempo do pulo

        if (soundEffect != null && !repeatSoundOnLoop)
            SFX.instance.PlaySFX(soundEffect, 1f);

        if(useCharacterAnimation && characterAnimator.runtimeAnimatorController != null)
            characterAnimator.SetTrigger(attackParameterName); //se tiver animação, usar ela
        for (int i = 0; i < quantidadesDeAtaque; i++) //determina quantos ataques devem ocorrer
        {
            if (efeitoSecundario != null) efeitoSecundario.ApplyEffect(alvo); //se tiver efeito secundario, ativar

            if (soundEffect != null && repeatSoundOnLoop) SFX.instance.PlaySFX(soundEffect, 1f);// tocar som do ataque

            if (ataqueEmArea) //se for, faz o ataque em area, se não, ataca normalmente
            {
                List<BasePersonagem> alvos = turnModeManager.turno == Turnos.PlayerTurn ? 
                    new List<BasePersonagem>(turnModeManager.inimigosPersonagens) 
                    : new List<BasePersonagem>(turnModeManager.aliadosPersonagens);

                for (int j = 0; j < alvos.Count; j++)
                {
                    MovesVisualEffect.instance.AttackEffect(attackSprite, alvos[j].transform.position, attackAnimation, animationPlayInFront, attackEffectYOffset);

                    if (efeitoSecundario != null)
                        efeitoSecundario.ApplyEffect(alvos[j]);

                    if (alvo != alvos[j])
                        alvos[j].TakeDamage(danoOuCura / 2, shakeCamera, quemEstaAtacando.isBuffed);
                    else
                        alvo.TakeDamage(Mathf.FloorToInt(danoOuCura * quemEstaAtacando.strengthFactor), shakeCamera, quemEstaAtacando.isBuffed);
                }
            } 
            else
            {
                MovesVisualEffect.instance.AttackEffect(attackSprite, alvoPos, attackAnimation, animationPlayInFront,attackEffectYOffset , 0.7f / quantidadesDeAtaque);

                alvo.TakeDamage(Mathf.FloorToInt(danoOuCura * quemEstaAtacando.strengthFactor), shakeCamera, quemEstaAtacando.isBuffed);
            }
            await Task.Delay((int)VisualEffectDuration * 1000 / quantidadesDeAtaque);
        }
        if (useCharacterAnimation && characterAnimator.runtimeAnimatorController != null)
            characterAnimator.SetTrigger("Exit");
    }
}