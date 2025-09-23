using UnityEngine;

[CreateAssetMenu(fileName = "PoisonEffect", menuName = "EfeitoSecundario/PoisonEffect", order = 1)]
public class PoisonEffect : Effect
{
    [SerializeField] BasePersonagem character;
    public int damagePerTurn;
    [SerializeField] RuntimeAnimatorController poisonAnimation;
    Color poisonColor = new Color(138f, 0f, 214f); // Cor roxa
    [SerializeField] AudioClip poisonSound;
    public override void ApplyEffect(BasePersonagem alvo)
    {
        character = alvo;
        remainingTurns = durationInTurn;
        var resultado = alvo.ChecarSeJaPossuiEfeito(this);
        StatusEffect alvoStatus = alvo.statusEffect;
        if (resultado.jaTem) //se o alvo ja tiver o efeito
        {
            resultado.efeitoQueJaPossui.remainingTurns = durationInTurn;
        }
        else //se o alvo não tiver o efeito
        {
            alvo.efeitosAtivos.Add(Instantiate(this));
           if(alvoStatus == StatusEffect.Nada)
            alvo.statusEffect = StatusEffect.Veneno;
        }
        character.GetComponent<SpriteRenderer>().color = poisonColor;
        character = alvo;
    }
    public override void OnTurnStart(BasePersonagem alvo)
    {
        if (remainingTurns <= durationInTurn)
        {
            remainingTurns--;
        }
        if (remainingTurns <= 0)
        {
            RemoveEffect();
        }
        SFX.instance.PlaySFX(poisonSound, 1f);
        MovesVisualEffect.instance.AttackEffect(null, character.transform.position, poisonAnimation, false);
        TextPopup.instance.GerarTexto("Envenenado!", character.transform.position, poisonColor, 29);
        character.TakeDamage(damagePerTurn, false);
        
    }
    public override void RemoveEffect()
    {
        character.efeitosAtivos.Remove(this);
        character.statusEffect = StatusEffect.Nada;
        character.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
