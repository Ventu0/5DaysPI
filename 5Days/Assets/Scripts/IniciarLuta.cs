using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
[System.Serializable]
public class Inimigo
{
    public bool jaMorreu;
    public Inimigo(bool jaMorreu)
    {
        this.jaMorreu = jaMorreu;
    }
}
public class IniciarLuta : MonoBehaviour
{
    public delegate void OnStartBattle();
    public OnStartBattle onStartBattle;

    [SerializeField] string cenaEscolhida;
    [SerializeField] CharacterStatusGeneric[] enemiesStatus;
    [SerializeField] float escapeChance = 0.7f; //chance de escapar da batalha, entre 0 e 1 

    public bool jaMorreu;
    public int personalID;
    public Inimigo inimigo;
    void Start()
    {
        inimigo = new Inimigo(jaMorreu);
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
        inimigo.jaMorreu = true;
        gameObject.SetActive(false);
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
        PauseMenuController.instance.canPause = false;

        DiaENoite dayScript = DiaENoite.instance;
        if (dayScript.clockUI != null) dayScript.clockUI.SetActive(false);
        dayScript.PauseTime(true);

        TurnModeManager turnModeManager = TurnModeManager.instance;
        turnModeManager.iniciarLuta = this;
        turnModeManager.escapeChance = escapeChance;

        RecieveInfoManager.instance.SetupCharacters(statusAtualizado,enemiesStatus.ToList());
    }
}