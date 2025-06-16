using UnityEngine;
using System.Threading.Tasks;
[CreateAssetMenu(menuName = "AtaqueNeutro/AtaqueNeutro")]
public class GolpesNeutros : Attack
{
    BasePersonagem alvoPersonagem;
    [Header("Exclusivo neutral")]
    [SerializeField] bool changeColorWhileApplyingEffect;
    [SerializeField] Color healingColor = Color.green;
    public override async void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite)
    {
        alvoPersonagem = alvo;
        Vector2 alvoPos = new Vector2(alvo.transform.position.x, alvo.transform.position.y + 0.5f);

        MovesVisualEffect.instance.AttackEffect(attackSprite, alvoPos, animation, animationPlayInFront, effectsDuration);
        if (changeColorWhileApplyingEffect) ChangeColorWhileHealing();
        await Task.Delay(Mathf.CeilToInt(effectsDuration) * 1000);

        if (efeitoSecundario != null)
            efeitoSecundario.ApplyEffect(alvo, danoOuCura);
        alvo.UpdateLife();

        if(soundEffect != null) SFX.instance.PlaySFX(soundEffect);
    }
    private async void ChangeColorWhileHealing()
    {
        SpriteRenderer alvoSpriteRenderer = alvoPersonagem.GetComponent<SpriteRenderer>();
        Color originalColor = alvoSpriteRenderer.color;
        alvoSpriteRenderer.color = healingColor;
        await Task.Delay(Mathf.CeilToInt(effectsDuration) * 1000);
        alvoSpriteRenderer.color = originalColor;
    }
}
