using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public string effectName;
    public int durationInTurn;

    public virtual void ApplyEffect(BasePersonagem alvo)
    {

    }
    public virtual void OnTurnStart()
    {

    }
    public virtual void RemoveEffect()
    {

    }
}
