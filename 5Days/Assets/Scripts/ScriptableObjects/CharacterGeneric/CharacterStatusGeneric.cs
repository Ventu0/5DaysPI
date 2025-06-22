using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterStatusGeneric", menuName = "Status Genérico/Novo Character")]
public class CharacterStatusGeneric : ScriptableObject
{
    public Sprite characterSprite;
    public int vidaMaxima;
    public int vidaAtual;
    public bool isDead = false;
    public List<Attack> ataques;
    public RuntimeAnimatorController animatorController;
    public bool isEnemy; // true se for inimigo, false se for aliado
}
