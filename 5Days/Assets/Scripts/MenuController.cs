using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.IO;


public class MenuController : MonoBehaviour
{
    [SerializeField] Button loadGameButton;

    [Header("Interação com o menu (animação)")]
    [SerializeField] Sprite clickSprite;
    [SerializeField] Transform characterTransform;
    [SerializeField] float jumpQuantity = 10f;
    [SerializeField] float bobbingDuration = 0.5f;
    Image objectImage;
    Sprite originalSprite;
    void Start()
    {
        objectImage = characterTransform.GetComponent<Image>();
        originalSprite = objectImage.sprite;
        loadGameButton.onClick.AddListener(LoadGameButton);
        loadGameButton.enabled = ChecarSePossuiSave();
        if (!ChecarSePossuiSave())
        {
            return;
        }
        else
        {
            Color deactivateColor = new Color(255, 255, 255, 110);
            loadGameButton.image.color = deactivateColor;
            loadGameButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = deactivateColor;
        }
    }
    #region MenuButtons
    public void NewGameButton()
    {
        if (ChecarSePossuiSave())
        {
            string pasta = Application.persistentDataPath;
            string[] arquivos = Directory.GetFiles(pasta, "*.json");

            for(int i = 0; i < arquivos.Length; i++)
            {
                File.Delete(arquivos[i]); //se já houver um save, deleta ele
            }
        }
        PlayerPrefs.DeleteAll();
        Loader.instance.DeletarTudo();
        SceneManager.LoadScene("CasaDianas");
    }
    public void LoadGameButton()
    {
        Loader.instance.Carregar();
    }
    public bool ChecarSePossuiSave()
    {
        string caminho = Application.persistentDataPath;
        string[] arquivosJson = Directory.GetFiles(caminho, "*.json");
        return arquivosJson.Length > 0;
    }
    public void OptionsButton()
    {

    }
    public void ExitButton()
    {
        Application.Quit();
    }
    #endregion
    public void InteractObject()
    {
        StartCoroutine(BounceEffect.instance.Bounce(objectImage.transform, bobbingDuration, jumpQuantity, OnEndBounce));
        objectImage.sprite = clickSprite;
    }
    void OnEndBounce()
    {
        objectImage.sprite = originalSprite;
    }
}
