using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
[CreateAssetMenu(menuName = "Ataques/AtaqueNeutro")]
public class GolpesNeutros : Attack
{
    BasePersonagem alvoPersonagem;
    public string buffType = "Cura: ";
    [Header("Exclusivo neutral")]
    [SerializeField] bool changeColorWhileApplyingEffect;
    [SerializeField] Color colorToChange = Color.green;
    public override async void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite) //tipo de alvo: O próprio usuário
    {
        alvoPersonagem = alvo;
        Debug.Log("Alvo: " + alvo.name);

        Vector2 alvoPos = new Vector2(alvo.transform.position.x, alvo.transform.position.y + 0.5f);
        TurnModeManager turnModeManager = TurnModeManager.instance;
        InteractButtonsController.instance.menu.SetActive(false);
        Animator characterAnimator = turnModeManager.QuemEstaAtacando().GetComponent<Animator>();
        if (ataqueEmArea)
        {
            List<BasePersonagem> alvos = EncontrarAliados();
            for (int i = 0; i < alvos.Count; i++)
            {
                MovesVisualEffect.instance.AttackEffect(attackSprite, alvos[i].transform.position, attackAnimation, animationPlayInFront, attackEffectYOffset, VisualEffectDuration);
                if (changeColorWhileApplyingEffect)
                    ChangeColorDuringEffect(alvos[i]);
            }
        }
        else
        {
            MovesVisualEffect.instance.AttackEffect(attackSprite, alvoPos, attackAnimation, animationPlayInFront, attackEffectYOffset, VisualEffectDuration);
            if (changeColorWhileApplyingEffect) ChangeColorDuringEffect(alvo);
        }
        currentPP -= 1;

        await Task.Delay(Mathf.CeilToInt(VisualEffectDuration) * 1000);

        if (useCharacterAnimation && characterAnimator.runtimeAnimatorController != null)
            characterAnimator.SetTrigger(attackParameterName); //se tiver animação, usar ela

        AplicarEfeito(alvo);

        if(soundEffect != null) SFX.instance.PlaySFX(soundEffect);

        turnModeManager.QuemEstaAtacando().EndTurn();
    }
    void AplicarEfeito(BasePersonagem alvo)
    {
        if(efeitoSecundario == null)
        {
            Debug.LogError("precisa ter um efeito pra aplicar ele, gênio!");
            return;
        }

        if (ataqueEmArea)
        {
            List<BasePersonagem> alvos = EncontrarAliados();
            for(int i = 0; i < alvos.Count; i++)
            {
                efeitoSecundario.ApplyEffect(alvos[i], danoOuCura);
                efeitoSecundario.ApplyEffect(alvos[i]);
                alvos[i].AtualizarVida();
            }
        }
        else
        {
            efeitoSecundario.ApplyEffect(alvo, danoOuCura);
            efeitoSecundario.ApplyEffect(alvo);
            alvo.AtualizarVida();
        }
        Debug.Log("terminando de aplicar o efeito");
    }
    public override List<BasePersonagem> EncontrarAliados()
    {
        return base.EncontrarAliados();
    }
    private async void ChangeColorDuringEffect(BasePersonagem alvo)
    {
        SpriteRenderer alvoSpriteRenderer = alvo.GetComponent<SpriteRenderer>();
        Color originalColor = alvoSpriteRenderer.color;
        alvoSpriteRenderer.color = colorToChange;
        await Task.Delay(Mathf.CeilToInt(VisualEffectDuration) * 1000);
        alvoSpriteRenderer.color = originalColor;
    }
}