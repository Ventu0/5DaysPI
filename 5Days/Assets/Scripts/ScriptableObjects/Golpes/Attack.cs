using UnityEngine;

public abstract class Attack : ScriptableObject
{
    public int dano;
    public string nomeAtaque;
    public Sprite iconeAtaque;
    public Sprite attackEffect;

    [Header("Configurações Opcionais")]
    public RuntimeAnimatorController animation;
    public bool animationPlayInFront = true;
    public Effect efeitoSecundario;
    public AudioClip soundEffect;

    [Header("Configurações de Ataque")]
    public bool shakeCamera;
    public bool ataqueEmArea;


    public virtual void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite)
    {

    }
}                                                                                                 