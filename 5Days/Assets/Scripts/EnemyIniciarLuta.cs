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
public class EnemyIniciarLuta : MonoBehaviour
{
    public delegate void OnStartBattle();
    public OnStartBattle onStartBattle;
    
    [SerializeField] string cenaEscolhida = "CombatScenes";
    [SerializeField] CharacterStatusGeneric[] enemiesStatus;
    [SerializeField] float escapeChance = 0.7f; //chance de escapar da batalha, entre 0 e 1 
    [SerializeField] int moneyYield = 5;
    public bool isEnemyPersistent = true;
    [HideInInspector] public bool battleStarted = false;
    public bool jaMorreu;
    public Inimigo inimigo;
    void Start()
    {
        inimigo = new Inimigo(jaMorreu);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (battleStarted) return;

        SceneManager.LoadScene(cenaEscolhida, LoadSceneMode.Additive);
        SceneTimeController.instance.sceneTime = 0;
        SceneTimeController.instance.PausarJogo();
        Invoke("ActivateBefore", 0.025f);
        Invoke("WaitSomeTime", 0.05f);
    }
    void ActivateBefore()
    {
        CombatBackground.instance.ChangeBackGround();
    }
    public void EndBattle()
    {
        battleStarted = false;
        inimigo.jaMorreu = true;
        gameObject.SetActive(false);
        if (PlayerMoney.instance != null)
        {
            PlayerMoney.instance.SetActive(true);
            PlayerMoney.instance.AddMoney(moneyYield);
        }
            
        Time.timeScale = 0f;
    }
    void WaitSomeTime()
    {

        PlayerPartyController party = PlayerPartyController.instance;
        TurnModeManager turnModeManager = TurnModeManager.instance;
        turnModeManager.currentFightingEnemy = this;
        turnModeManager.escapeChance = escapeChance;

        List<CharacterStatusGeneric> statusAtualizado = new List<CharacterStatusGeneric>();
        for (int i = 0; i < party.partyAtual.Count; i++)
        {
            statusAtualizado.Add(party.partyAtual[i]);
        }
        MainSoundtrack.instance.ChooseRandomBattleSoundtrack();
        QuestController.instance.SetAllActive(false);
        PauseMenuController.instance.canPause = false;

        if(PlayerMoney.instance != null)
        PlayerMoney.instance.SetActive(false);

        Player.instance.canMove = false;
        battleStarted = true;

        DiaENoite dayScript = DiaENoite.instance;
        if (dayScript.clockUI != null) dayScript.clockUI.SetActive(false);
        dayScript.PauseTime(true);

        
        RecieveInfoManager.instance.SetupCharacters(statusAtualizado,enemiesStatus.ToList());
    }
}