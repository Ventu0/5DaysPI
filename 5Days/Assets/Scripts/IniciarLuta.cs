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
    [SerializeField] float escapeChance = 0.7f; //chance de escapar da batalha, entre 0 e 1 
    void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SceneManager.LoadScene(cenaEscolhida, LoadSceneMode.Additive);
        SceneTimeController.instance.sceneTime = 0;
        SceneTimeController.instance.PausarJogo();
        Invoke("WaitSomeTime", 0.3f);
    }
    public void EndBattle()
    {
        Destroy(gameObject);
        Time.timeScale = 0f;
    }
    void WaitSomeTime()
    {
        PlayerPartyController party = PlayerPartyController.instance;
        List<CharacterStatusGeneric> statusAtualizado = new List<CharacterStatusGeneric>();
        for (int i = 0; i < party.partyAtual.Count; i++)
        {
            if (!party.partyAtual[i].isDead)
            {
                statusAtualizado.Add(party.partyAtual[i]);
            }
        }
        QuestController.instance.menu.SetActive(false);
        DiaENoite dayScript = DiaENoite.instance;
        if (dayScript.clockUI != null) dayScript.clockUI.SetActive(false);
        dayScript.PauseTime(true);
        TurnModeManager.instance.iniciarLuta = this;
        TurnModeManager.instance.escapeChance = escapeChance;
        RecieveInfoManager.instance.SetupCharacters(statusAtualizado,enemiesStatus.ToList());
    }
}