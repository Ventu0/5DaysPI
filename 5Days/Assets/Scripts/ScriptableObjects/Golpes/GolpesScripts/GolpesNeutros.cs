using UnityEngine;
using System.Threading.Tasks;
[CreateAssetMenu(menuName = "AtaqueNeutro/AtaqueNeutro")]
public class GolpesNeutros : Attack
{
    BasePersonagem alvoPersonagem;
    [Header("Exclusivo neutral")]
    [SerializeField] bool changeColorWhileApplyingEffect;
    [SerializeField] Color colorToChange = Color.green;
    public override async void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite) //tipo de alvo: O próprio usuário
    {
        alvoPersonagem = alvo;
        Vector2 alvoPos = new Vector2(alvo.transform.position.x, alvo.transform.position.y + 0.5f);
        InteractButtonsController.instance.menu.SetActive(false);
        MovesVisualEffect.instance.AttackEffect(attackSprite, alvoPos, animation, animationPlayInFront, VisualEffectDuration);
        if (changeColorWhileApplyingEffect) ChangeColorDuringEffect();
        currentPP -= 1;
        await Task.Delay(Mathf.CeilToInt(VisualEffectDuration) * 1000);

        if (efeitoSecundario != null)
        {
            efeitoSecundario.ApplyEffect(alvo, danoOuCura);
            efeitoSecundario.ApplyEffect(alvo);
        }
        alvo.AtualizarVida();

        if(soundEffect != null) SFX.instance.PlaySFX(soundEffect);
        alvo.EndTurn();
    }
    private async void ChangeColorDuringEffect()
    {
        SpriteRenderer alvoSpriteRenderer = alvoPersonagem.GetComponent<SpriteRenderer>();
        Color originalColor = alvoSpriteRenderer.color;
        alvoSpriteRenderer.color = colorToChange;
        await Task.Delay(Mathf.CeilToInt(VisualEffectDuration) * 1000);
        alvoSpriteRenderer.color = originalColor;
    }
}
