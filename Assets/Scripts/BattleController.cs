using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    public Animator LunaAnimator;
    public Transform LunaTransform;
    public Transform MonsterTransform;
    public GameObject SkillEffect;
    public GameObject HealEffect;
    public GameObject Tip;
    public GameObject PreBattlePanel;
    public Text PreBattleText;
    public AudioClip LunaAttackSound;
    public AudioClip LunaAttackShout;
    public AudioClip MonsterAttackSound;
    public AudioClip MonsterHitSound;
    public AudioClip LunaHitSound;
    public AudioClip LunaDieSound;
    public AudioClip LunaHealSound;
    public AudioClip LunaSkillSound;
    Coroutine _pAL, _pRHPL, _pDL, _pSL, _mA;
    Vector3 _monsterPostion;
    Vector3 _lunaPostion;
    SpriteRenderer _monsterSR;
    Color _color, _pcolor;
    SpriteRenderer _lunaSR;
    void Awake()
    {
        _monsterPostion = MonsterTransform.localPosition;
        _lunaPostion = LunaTransform.localPosition;
        _monsterSR = MonsterTransform.gameObject.GetComponent<SpriteRenderer>();
        _lunaSR = LunaTransform.gameObject.GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        PreBattlePanel.SetActive(true);
        _color = PreBattleText.color;
        _pcolor = PreBattlePanel.GetComponent<Image>().color;
        _color.a = 255;
        _pcolor.a = 255;
        PreBattlePanel.GetComponent<Image>().color = _pcolor;
        PreBattleText.color = _color;
        StartCoroutine(FadeIn());
        UIManager.Instance.StatusOfBattleMenu(true);
        GameManager.Instance.ChangeMonsterHP(4);
        _lunaSR.DOFade(1, 0.01f);
        _monsterSR.DOFade(1, 0.01f);
        MonsterTransform.localPosition = _monsterPostion;
        LunaTransform.localPosition = _lunaPostion;
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void LunaAttack()
    {
        _pAL = StartCoroutine(_performAttackLogic());
    }
    public void LunaDefend()
    {
        _pDL = StartCoroutine(_performDefendLogic());
    }
    public void LunaSkill()
    {
        if (GameManager.Instance.DetectMP(3))
        {
            Tip.SetActive(true);
            CancelInvoke("HideTip");
            Invoke("HideTip", 2f);
        }
        else
        {
            _pSL = StartCoroutine(_performSkillLogic());
        }
    }
    public void LunaRecoverHP()
    {
        if (GameManager.Instance.DetectMP(2))
        {
            Tip.SetActive(true);
            CancelInvoke("HideTip");
            Invoke("HideTip", 2f);
        }
        else
        {
            _pRHPL = StartCoroutine(_performRecoverHPLogic());
        }
    }
    void HideTip()
    {
        Tip.SetActive(false);
    }
    public void LunaEscape()
    {
        UIManager.Instance.StatusOfBattleMenu(false);
        LunaAnimator.SetBool("MoveState", true);
        LunaAnimator.SetFloat("MoveValue", 1);
        LunaTransform.DOLocalMove(_lunaPostion + new Vector3(5, 0, 0), 0.5f).OnComplete(
               () =>
            {
                UIManager.Instance.EndPanel.SetActive(true);
                Color Ecolor = UIManager.Instance.EndPanel.GetComponent<Image>().color;
                Ecolor = new Color(0, 1, 0, 0.26f);
                UIManager.Instance.EndPanel.GetComponent<Image>().color = Ecolor;
                UIManager.Instance.Summary.text = "逃\t跑\t成\t功";
                GameManager.Instance.EnterOrExitBattle(false, GameManager.Instance.Luna, GameManager.Instance.Monster);
                GameManager.Instance.PlayMusic(GameManager.Instance.NormalClip);
            }
            );
    }
    void _judgeMonsterHP(int value)
    {
        GameManager.Instance.ChangeMonsterHP(value);
        if (GameManager.Instance.MonsterHP == 0)
        {
            _monsterSR.DOFade(0, 0.6f).OnComplete(
                () =>
                {
                    UIManager.Instance.EndPanel.SetActive(true);
                    Color Ecolor = UIManager.Instance.EndPanel.GetComponent<Image>().color;
                    Ecolor = new Color(0, 1, 0, 0.26f);
                    UIManager.Instance.EndPanel.GetComponent<Image>().color = Ecolor;
                    UIManager.Instance.Summary.text = "战\t斗\t胜\t利";
                    GameManager.Instance.EnterOrExitBattle(false, GameManager.Instance.Luna, GameManager.Instance.Monster);
                    GameManager.Instance.PlayMusic(GameManager.Instance.NormalClip);
                });
        }
        else
        {
            _monsterSR.DOFade(1, 0.2f);
        }
    }
    void _judgePlayerHP(int value)
    {
        GameManager.Instance.ChangeLunaHP(value);
        if (GameManager.Instance.LunaHP == 0)
        {
            LunaAnimator.CrossFade("Die", 0.5f);
            GameManager.Instance.PlaySound(LunaDieSound);
            _lunaSR.DOFade(0, 1f).OnComplete(
                () =>
                {
                    UIManager.Instance.StatusOfBattleMenu(false);
                    UIManager.Instance.EndPanel.SetActive(true);
                    Color Ecolor = UIManager.Instance.EndPanel.GetComponent<Image>().color;
                    Color Tcolor = UIManager.Instance.Summary.color;
                    Ecolor = new Color(1, 0, 0, 0.26f);
                    Tcolor = new Color(1, 0, 0, 1f);
                    UIManager.Instance.EndPanel.GetComponent<Image>().color = Ecolor;
                    UIManager.Instance.Summary.color = Tcolor;
                    UIManager.Instance.Summary.text = "战\t斗\t失\t败";
                    GameManager.Instance.EnterOrExitBattle(false, GameManager.Instance.Luna, GameManager.Instance.Monster);
                    GameManager.Instance.ChangeLunaHP(1);
                    GameManager.Instance.PlayMusic(GameManager.Instance.NormalClip);
                });
        }
    }
    IEnumerator _performAttackLogic()
    {
        UIManager.Instance.StatusOfBattleMenu(false);
        LunaAnimator.SetBool("MoveState", true);
        LunaAnimator.SetFloat("MoveValue", -1);
        LunaTransform.DOLocalMove(_monsterPostion + new Vector3(1, 0, 0), 0.5f).OnComplete
            (
             () =>
             {
                 GameManager.Instance.PlaySound(LunaAttackSound);
                 GameManager.Instance.PlaySound(LunaAttackShout);
                 LunaAnimator.SetBool("MoveState", false);
                 LunaAnimator.SetFloat("MoveValue", 0);
                 LunaAnimator.CrossFade("Attack", 0);
                 _monsterSR.DOFade(0.3f, 0.2f).OnComplete(() =>
                 {
                     _judgeMonsterHP(-1);
                     GameManager.Instance.PlaySound(MonsterHitSound);
                 });
             }
            );
        yield return new WaitForSeconds(1.2f);
        LunaAnimator.SetBool("MoveState", true);
        LunaAnimator.SetFloat("MoveValue", 1);
        LunaTransform.DOLocalMove(_lunaPostion, 0.5f).OnComplete
            (
            () =>
            {
                LunaAnimator.SetBool("MoveState", false);
                LunaAnimator.SetFloat("MoveValue", 0);
            }
            );
        yield return new WaitForSeconds(0.5f);
        if (GameManager.Instance.MonsterHP != 0)
        {
            _mA = StartCoroutine(_monsterAttack());
        }
    }
    IEnumerator _performDefendLogic()
    {
        UIManager.Instance.StatusOfBattleMenu(false);
        LunaAnimator.SetBool("Defend", true);
        MonsterTransform.DOLocalMove(_lunaPostion - new Vector3(1.5f, 0, 0), 0.5f);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.PlaySound(MonsterAttackSound);
        MonsterTransform.DOLocalMove(_lunaPostion, 0.2f).OnComplete
            (
             () =>
             {
                 MonsterTransform.DOLocalMove(_lunaPostion - new Vector3(1.5f, 0, 0), 0.2f);
                 LunaTransform.DOLocalMove(_lunaPostion + new Vector3(1.0f, 0, 0), 0.2f).OnComplete
            (
            () =>
            {
                GameManager.Instance.PlaySound(LunaHitSound);
                LunaTransform.DOLocalMove(_lunaPostion, 0.2f);
                LunaAnimator.SetBool("Defend", false);
            }
            ); ;
                 _lunaSR.DOFade(0.3f, 0.2f).OnComplete(() => { _lunaSR.DOFade(1, 0.2f); });
                 _judgePlayerHP(-1);
             }
            );
        yield return new WaitForSeconds(0.4f);
        MonsterTransform.DOLocalMove(_monsterPostion, 0.5f).OnComplete(
            () =>
            {
                UIManager.Instance.StatusOfBattleMenu(true);
            }
            );
    }
    IEnumerator _performSkillLogic()
    {
        UIManager.Instance.StatusOfBattleMenu(false);
        LunaAnimator.CrossFade("Skill", 0);
        GameManager.Instance.ChangeMP(-2);
        yield return new WaitForSeconds(0.35f);
        GameObject Skill = Instantiate(SkillEffect, MonsterTransform);
        Skill.transform.localPosition = Vector3.zero;
        GameManager.Instance.PlaySound(LunaSkillSound);
        yield return new WaitForSeconds(0.5f);
        _monsterSR.DOFade(0.3f, 0.2f).OnComplete(() =>
        {
            _judgeMonsterHP(-2);
            GameManager.Instance.PlaySound(MonsterHitSound);
        });
        yield return new WaitForSeconds(0.6f);
        if (GameManager.Instance.MonsterHP != 0)
        {
            _mA = StartCoroutine(_monsterAttack());
        }
    }
    IEnumerator _performRecoverHPLogic()
    {
        UIManager.Instance.StatusOfBattleMenu(false);
        LunaAnimator.CrossFade("RecoverHP", 0);
        yield return new WaitForSeconds(0.2f);
        GameObject Heal = Instantiate(HealEffect, LunaTransform);
        Heal.transform.localPosition = Vector3.zero;
        GameManager.Instance.ChangeMP(-2);
        GameManager.Instance.ChangeLunaHP(2);
        GameManager.Instance.PlaySound(LunaHealSound);
        yield return new WaitForSeconds(2.5f);
        _mA = StartCoroutine(_monsterAttack());
    }
    IEnumerator _monsterAttack()
    {
        MonsterTransform.DOLocalMove(_lunaPostion - new Vector3(1.5f, 0, 0), 0.5f);
        GameManager.Instance.PlaySound(MonsterAttackSound);
        yield return new WaitForSeconds(0.5f);
        MonsterTransform.DOLocalMove(_lunaPostion, 0.2f).OnComplete
            (
             () =>
             {
                 MonsterTransform.DOLocalMove(_lunaPostion - new Vector3(1.5f, 0, 0), 0.2f);
                 LunaAnimator.CrossFade("Hit", 0.5f);
                 _lunaSR.DOFade(0.3f, 0.2f).OnComplete(() => { _lunaSR.DOFade(1, 0.2f); });
             }
            );
        yield return new WaitForSeconds(0.7f);
        _judgePlayerHP(-2);
        MonsterTransform.DOLocalMove(_monsterPostion, 0.5f).OnComplete(
            () =>
            {
                UIManager.Instance.StatusOfBattleMenu(true);
            }
            );
    }
    IEnumerator FadeIn()
    {
        _color = PreBattleText.color;
        _pcolor = PreBattlePanel.GetComponent<Image>().color;
        float time = 0f;
        while (time < 2)
        {
            time += Time.deltaTime;
            _color.a = Mathf.Lerp(0, 1, time / 2);
            PreBattleText.color = _color;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        time = 0f;
        while (time < 2)
        {
            time += Time.deltaTime;
            _color.a = Mathf.Lerp(1, 0, time / 2);
            _pcolor.a = Mathf.Lerp(1, 0, time / 2);
            PreBattleText.color = _color;
            PreBattlePanel.GetComponent<Image>().color = _pcolor;
            yield return null;
        }
        yield return null;
        PreBattlePanel.SetActive(false);
    }
}
