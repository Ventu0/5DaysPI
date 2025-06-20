using UnityEngine;

public enum StatusEffect
{
    Nada,
    Veneno,
    Atordoado
}
public abstract class Effect : ScriptableObject
{
    public string effectName;
    public int durationInTurn;
    [HideInInspector] public int remainingTurns;

    public virtual void ApplyEffect(BasePersonagem alvo)
    {


    }
    public virtual void ApplyEffect(BasePersonagem alvo, int buffDebuffNumberQuantity = 0)
    {

    }
    public virtual void OnTurnStart(BasePersonagem alvo)
    {
  
    }
    public virtual void RemoveEffect()
    {

    }
    public virtual void OneTurnActivation()
    {

    }
}
