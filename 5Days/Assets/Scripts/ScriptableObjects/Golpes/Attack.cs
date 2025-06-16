using UnityEngine;
public enum Alvo
{
    Inimigo,
    Aliado,
    Self
}
public abstract class Attack : ScriptableObject
{
    public int danoOuCura;
    public string nomeAtaque;
    public Sprite iconeAtaque;
    public Sprite attackEffect;
    public Alvo tipoDeAlvo;

    [Header("Configurações Opcionais")]
    public RuntimeAnimatorController animation;
    public bool animationPlayInFront = true;
    public Effect efeitoSecundario;
    public AudioClip soundEffect;

    [Header("Configurações de Ataque")]
    public bool ataqueNeutro;
    [Tooltip("Ataque que buffa ou debuffa o alvo, não causa dano")]
    public bool shakeCamera;
    public bool ataqueEmArea;


    public virtual void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite)
    {

    }
    public virtual void AplicarDebuff(BasePersonagem alvo)
    {
        
    }
}                                                                                                 