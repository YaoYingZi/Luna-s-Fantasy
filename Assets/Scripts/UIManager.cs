using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Image HPMask;
    public Image MPMask;
    public Image MonsterHPMask;
    public GameObject BattleMenu;
    public GameObject TalkPanel;
    public GameObject StartPanel;
    public GameObject EndPanel;
    public GameObject ExitPanel;
    public Text Summary;
    public Text Author;
    public Image CharacterImage;
    public Sprite[] CharacterSprites;
    public Text NameText;
    public Text ContentText;
    private float _originalSize;
    private float _originalMonsterSize;
    void Awake()
    {
        Instance = this;
        _originalSize=HPMask.rectTransform.rect.width;
        _originalMonsterSize = MonsterHPMask.rectTransform.rect.width;
        SetMonsterHPValue(1);
    }
    private void Start()
    {
        SetHPValue((float)(GameManager.Instance.LunaHP) / (GameManager.Instance.LunaMaxHP));
        SetMPValue((float)(GameManager.Instance.LunaMP) / (GameManager.Instance.LunaMaxMP));
        StartCoroutine(FadeIn());
    }
    public void SetHPValue(float fillPercent)
    {
        HPMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fillPercent*_originalSize);
    }
    public void SetMonsterHPValue(float fillPercent)
    {
        MonsterHPMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fillPercent * _originalMonsterSize);
    }
    public void SetMPValue(float fillPercent)
    {
        MPMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fillPercent*_originalSize);
    }
    public void StatusOfBattleMenu(bool status)
    {
        BattleMenu.SetActive(status); 
    }
    public void ExitBattleSummary()
    {
        EndPanel.SetActive(false);
    }
    public void ShowDialog(string content = null, string name = null)
    {
        //¹Ø±Õ
        if (content == null)
        {
            TalkPanel.SetActive(false);
        }
        else
        {
            TalkPanel.SetActive(true);
            if (name != null)
            {
                if (name == "Luna")
                {
                    CharacterImage.sprite = CharacterSprites[0];
                }
                else
                {
                    CharacterImage.sprite = CharacterSprites[1];
                }
                CharacterImage.SetNativeSize();
            }
            ContentText.text = content;
            NameText.text = name;
        }
    }
    public void ExitGame()
    {
        ExitPanel.SetActive(false);
        GameManager.Instance.ExitGame = !GameManager.Instance.ExitGame;
    }
    public void ConfirmExitGame()
    {
        Application.Quit();
    }
    IEnumerator FadeIn()
    {
        float time = 0f;
        Color color = Author.color;
        Color Scolor = StartPanel.GetComponent<Image>().color;

        while (time < 8)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, time / 10);
            Author.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        time = 0f;
        while (time < 5)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, time / 5);
            Scolor.a = Mathf.Lerp(1, 0, time / 5);
            Author.color = color;
            StartPanel.GetComponent<Image>().color = Scolor;
            yield return null;
        }
        yield return null;
        StartPanel.SetActive(false);
    }
}
