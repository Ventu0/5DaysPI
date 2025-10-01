using UnityEngine;

[CreateAssetMenu(fileName = "PersonagensNaLoja", menuName = "Scriptable Objects/PersonagensNaLoja")]
public class PersonagensNaLoja : ScriptableObject
{
    public string nomePersonagem;
    public Sprite display;
    public CharacterStatusGeneric personagemOriginal;
}
