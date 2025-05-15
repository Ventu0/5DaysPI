using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterStatusGeneric", menuName = "Status Genérico/Novo Inimigo")]
public class CharacterStatusGeneric : ScriptableObject
{
    public Sprite characterSprite;
    public int força;
    public int defesa;
    public int vidaMaxima;
    public int vidaAtual;
    public List<Attack> ataques;
    public RuntimeAnimatorController animatorController;
    
}
