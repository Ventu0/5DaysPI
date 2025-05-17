using UnityEngine;

public abstract class Attack : ScriptableObject
{
    public int dano;
    public string nomeAtaque;
    public Sprite iconeAtaque;
    public Sprite attackEffect;
    [Tooltip("(OPCIONAL) Em caso de uma animação, coloque a animação")]
    public RuntimeAnimatorController animation;
    

    public virtual void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite)
    {

    }
}
