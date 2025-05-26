using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterStatusGeneric", menuName = "Status Genérico/Novo Character")]
public class CharacterStatusGeneric : ScriptableObject
{
    public Sprite characterSprite;
    public int força;
    public int defesa;
    public int vidaMaxima;
    public int vidaAtual;
    public List<Attack> ataques;
    public RuntimeAnimatorController animatorController;
    public bool isEnemy; // true se for inimigo, false se for aliado
}
