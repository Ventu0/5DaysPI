using System.Collections.Generic;
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
    public float attackEffectYOffset = -0.5f;
    public Alvo tipoDeAlvo;

    [Header("Configurações Opcionais")]
    public RuntimeAnimatorController attackAnimation;
    public bool animationPlayInFront = true;
    public Effect efeitoSecundario;
    [Range(0, 1)]
    public float porcentagemDeCura = 0.3f;
    [Tooltip("Se o ataque for de cura, qual a porcentagem de cura que ele vai fazer")]
    public AudioClip soundEffect;
    public bool repeatSoundOnLoop = true;

    [Header("Situacional")]
    public bool canUseSelectMenu = true;

    [Header("Configurações de Ataque")]
    public bool usarMovimentoLinear;
    [Tooltip("Se sim, se mexe ao inimigo caminhando. Se não, pula até o inimigo")]
    public bool ataqueUmaVezSó = false;
    public bool oneTime;
    public bool shakeCamera;
    public bool ataqueEmArea;
    public bool stealHeal;
    [Tooltip("se for um ataque que cura, precisa ter efeito secundario de cura")]
    public bool useCharacterAnimation = false;
    [Tooltip("se o personagem tiver animation, usar ela")]

    [Header("Configurações da animação do personagem (ativar se useCharacterAnimation for true)" )]
    public string attackParameterName;
    public string endAttackParameter;
    public virtual List<BasePersonagem> EncontrarAliados()
    { //fiz esse if pra eu identificar quem esta atacando, pra os inimigos poderem usar esses golpes também
        TurnModeManager turnModeManager = TurnModeManager.instance;
        List<BasePersonagem> alvos = new List<BasePersonagem>();

        switch (turnModeManager.turno)
        {
            case Turnos.EnemyTurn:

                if (tipoDeAlvo == Alvo.Aliado)
                    alvos = turnModeManager.inimigosPersonagens;
                else if (tipoDeAlvo == Alvo.Inimigo)
                    alvos = turnModeManager.aliadosPersonagens;

                break;

            case Turnos.PlayerTurn:

                if (tipoDeAlvo == Alvo.Inimigo)
                    alvos = turnModeManager.inimigosPersonagens;
                else if (tipoDeAlvo == Alvo.Aliado)
                    alvos = turnModeManager.aliadosPersonagens;

                break;

            default:
                Debug.Log("Alvo é self");
                alvos.Add(turnModeManager.QuemEstaAtacando());
                break;
        }
        return new List<BasePersonagem>(alvos);
    }
    public virtual void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite)
    {

    }
    public virtual void AplicarDebuff(BasePersonagem alvo)
    {
        
    }
}                                                                                                 