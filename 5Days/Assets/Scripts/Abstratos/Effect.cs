using UnityEngine;

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
    public virtual void OnTurnStart()
    {
        if (remainingTurns <= durationInTurn)
        {
            remainingTurns--;
        }
        if (remainingTurns <= 0)
        {
            RemoveEffect();
        }
    }
    public virtual void RemoveEffect()
    {

    }
    public virtual void OneTurnActivation()
    {

    }
}
