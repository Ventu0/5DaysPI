using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu(menuName = "Venenos/Novo veneno")]
public class PoisonEffect : Effect
{
    [HideInInspector] public BasePersonagem character;
    public int damagePerTurn;
    [SerializeField] int remainingTurns;
    public override void ApplyEffect(BasePersonagem alvo)
    {
        character = alvo;
        remainingTurns = durationInTurn;
        alvo.efeitoAtivo = Instantiate(this);
        character.GetComponent<SpriteRenderer>().color = new Color(128, 0, 128);
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
        character.TakeDamage(damagePerTurn, false);
        
    }
    public override void RemoveEffect()
    {
        character.efeitoAtivo = null;
        character.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
