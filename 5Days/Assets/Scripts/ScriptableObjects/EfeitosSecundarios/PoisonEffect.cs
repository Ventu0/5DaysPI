using UnityEngine;

[CreateAssetMenu(menuName = "Venenos/Novo veneno")]
public class PoisonEffect : Effect
{
    [HideInInspector] public BasePersonagem character;
    public int damagePerTurn;
    [SerializeField] int remainingTurns;
    [SerializeField] RuntimeAnimatorController poisonAnimation;
    Color poisonColor = new Color(138f, 0f, 214f); // Cor roxa
    public override void ApplyEffect(BasePersonagem alvo)
    {
        character = alvo;
        remainingTurns = durationInTurn;
        alvo.efeitoAtivo = Instantiate(this);
        character.GetComponent<SpriteRenderer>().color = poisonColor;
    }
    public override void OnTurnStart()
    {
        if(remainingTurns <= durationInTurn)
        {
            remainingTurns--;
        }
        if(remainingTurns <= 0)
        {
            RemoveEffect();
        }
        MovesVisualEffect.instance.AttackEffect(null, character.transform.position, poisonAnimation, false);
        TextPopup.instance.GerarTexto("Envenenado!", character.transform.position, 29, poisonColor);
        character.TakeDamage(damagePerTurn, false);
        
    }
    public override void RemoveEffect()
    {
        character.efeitoAtivo = null;
        character.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
