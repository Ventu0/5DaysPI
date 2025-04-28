using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class IniciarLuta : MonoBehaviour
{
    [SerializeField] string cenaEscolhida;
    [SerializeField] CharacterStatusGeneric[] enemiesStatus;
    [SerializeField] List<CharacterStatusGeneric> playerParty;
    void Start()
    {
        playerParty = Player.instance.partyStatus;
        for (int i = 0; i < enemiesStatus.Length; i++)
        {
            print("ADICIONANDO O COISA");
            TurnModeInfo.enemiesToLoad.Add(enemiesStatus[i]);
        }
        for (int i = 0; i < Player.instance.partyStatus.Count; i++)
        {
            TurnModeInfo.alliesToLoad.Add(playerParty[i]);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SceneManager.LoadScene(cenaEscolhida, LoadSceneMode.Additive);
        SceneTimeController.instance.sceneTime = 0;
    }
}
public static class TurnModeInfo
{
    public static List<CharacterStatusGeneric> enemiesToLoad = new List<CharacterStatusGeneric>();
    public static List<CharacterStatusGeneric> alliesToLoad = new List<CharacterStatusGeneric>();
}