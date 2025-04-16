using UnityEngine;

public abstract class Attack : ScriptableObject
{
    public int dano;
    public string nomeAtaque;
    public RuntimeAnimatorController animatorController;

    public virtual void ExecutarAtaque(BasePersonagem alvo)
    {

    }
}
