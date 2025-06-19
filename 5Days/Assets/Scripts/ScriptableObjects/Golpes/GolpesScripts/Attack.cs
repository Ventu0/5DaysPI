using UnityEngine;
public enum Alvo
{
    Inimigo,
    Aliado,
    Self
}
public abstract class Attack : ScriptableObject
{
    [Header("Configurações Base")]
    public int danoOuCura;
    public float effectsDuration = 1f;
    [Tooltip("tempo em segundos para a duração do efeito visual")]
    public int quantidadesDeAtaque = 1;
    [Tooltip("Quantidades de ataque executado em sequência")]
    public string nomeAtaque;
    public Sprite iconeAtaque;
    public Color iconMainColor = Color.yellow;
    public Sprite attackEffect;
    [Tooltip("Sprite do efeito visual (caso não possua animação, isto é obrigatório")]
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