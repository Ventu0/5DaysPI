using UnityEngine;

public abstract class Attack : ScriptableObject
{
    public int dano;
    public string nomeAtaque;
    public bool shakeCamera;
    public Sprite iconeAtaque;
    public Sprite attackEffect;
    [Tooltip("(OPCIONAL) Em caso de uma animação, coloque a animação")]
    public RuntimeAnimatorController animation;
    public Effect efeitoSecundario;
    public AudioClip soundEffect;
    [Header("Configurações de Ataque")]
    public bool ataqueEmArea;


    public virtual void ExecutarAtaque(BasePersonagem alvo, Sprite attackSprite)
    {

    }
}                                                                                                 