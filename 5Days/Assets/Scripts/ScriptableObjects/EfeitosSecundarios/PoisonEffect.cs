using UnityEngine;

[CreateAssetMenu(menuName = "Venenos/Novo veneno")]
public class PoisonEffect : Effect
{
    [HideInInspector] public BasePersonagem character;
    public int damagePerTurn;
    int remainingTurns;
    public override void ApplyEffect(BasePersonagem alvo)
    {
        character = alvo;
        remainingTurns = durationInTurn;
        alvo.efeitoAtivo = this;
        character.GetComponent<SpriteRenderer>().color = Color.green;
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
        Debug.Log("Tomando dano do veneno!");
        character.TakeDamage(damagePerTurn, false);
        
    }
    public override void RemoveEffect()
    {
        character.efeitoAtivo = null;
        character.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
