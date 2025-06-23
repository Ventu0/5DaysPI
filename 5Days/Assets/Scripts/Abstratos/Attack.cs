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
    public string nomeAtaque;
    public string description;
    public int danoOuCura;
    public int maxPP = 15;
    public int currentPP = 13;
    public float VisualEffectDuration = 1f;
    [Tooltip("tempo em segundos para a duração do efeito visual")]
    public Sprite iconeAtaque;
    public Color iconMainColor = Color.yellow;
    public Sprite attackEffect;
    [Tooltip("Sprite do efeito visual (caso não possua animação, isto é obrigatório)")]
    public Alvo tipoDeAlvo;

    [Header("Configurações Opcionais")]
    public RuntimeAnimatorController animation;
    public bool animationPlayInFront = true;
    public Effect efeitoSecundario;
    public AudioClip soundEffect;

    [Header("Configurações de Ataque")]
    public bool usarMovimentoLinear;
    [Tooltip("Se sim, se mexe ao inimigo caminhando. Se não, pula até o inimigo")]
    public bool ataqueUmaVezSó = false;
    public bool shakeCamera;
    public bool ataqueEmArea;
    [HideInInspector] public bool oneTime;

    public virtual void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite)
    {

    }
    public virtual void AplicarDebuff(BasePersonagem alvo)
    {
        
    }
}                                                                                                 