using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class IniciarLuta : MonoBehaviour
{
    public delegate void OnStartBattle();
    public OnStartBattle onStartBattle;
    [SerializeField] string cenaEscolhida;
    [SerializeField] CharacterStatusGeneric[] enemiesStatus;
    [SerializeField] List<CharacterStatusGeneric> playerParty;
    void Start()
    {
        playerParty = Player.instance.partyStatus;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SceneManager.LoadScene(cenaEscolhida, LoadSceneMode.Additive);
        Invoke("WaitSomeTime", 0.5f);
    }
    void WaitSomeTime()
    {
        RecieveInfoManager.instance.SetupCharacters(playerParty, enemiesStatus.ToList());
        SceneTimeController.instance.sceneTime = 0;
    }
}