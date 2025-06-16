using UnityEngine;
using System.Threading.Tasks;
[CreateAssetMenu(menuName = "AtaqueNeutro/AtaqueNeutro")]
public class GolpesNeutros : Attack
{
    public override async void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite)
    {
        Vector2 alvoPos = new Vector2(alvo.transform.position.x, alvo.transform.position.y + 0.5f);

        await Task.Delay(Mathf.CeilToInt(TurnModeManager.instance.QuemEstaAtacando().duration) * 250);

        if (efeitoSecundario != null)
            efeitoSecundario.ApplyEffect(alvo, danoOuCura);

        MovesVisualEffect.instance.AttackEffect(attackSprite, alvoPos, animation, animationPlayInFront);
        if(soundEffect != null) SFX.instance.PlaySFX(soundEffect);
    }
}
