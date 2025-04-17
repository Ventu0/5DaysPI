using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterStatusGeneric", menuName = "Status Genérico/Novo Inimigo")]
public class CharacterStatusGeneric : ScriptableObject
{
    public int força;
    public int vida;
    public List<Attack> ataques;
}
