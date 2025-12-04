using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterStatusGeneric", menuName = "Status Genérico/Novo Character")]
public class CharacterStatusGeneric : ScriptableObject
{
    public Sprite characterSprite;
    public int vidaMaxima;
    public int vidaAtual;
    public float YOffset = 0;
    [Tooltip("posicao Y do personagem em relacao a sombra. Positivo para cima, negativo para baixo")]
    public int spriteSort = 0;
    [Tooltip("Layer do boneco")]
    public bool isDead = false;
    public List<Attack> ataques;
    public RuntimeAnimatorController animatorController;
    public bool flipX; 
}
