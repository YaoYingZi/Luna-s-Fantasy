using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject BattleGround;
    public NPCDialogue NalaDialogue;
    public int DialogInfoIndex;
    public bool CanControlLuna;
    public bool HasPetTheDog;
    public int CandleNum;
    public int KillNum;
    public bool ExitGame;
    public GameObject Monsters;
    private int _lunaHP;
    private int _lunaMaxHP;
    private int _lunaMP;
    private int _lunaMaxMP;
    private int _monsterHP;
    private int _monsterMaxHP;
    private GameObject _luna;
    private GameObject _monster;
    public AudioSource AudioManager;
    public AudioClip NormalClip;
    public AudioClip BattleClip;
    public GameObject Luna { get { return _luna; } set { _luna = value; } }
    public GameObject Monster { get { return _monster; } set { _monster = value; } }
    public int LunaHP { get { return _lunaHP; } }
    public int LunaMaxHP { get { return _lunaMaxHP; } }
    public int LunaMP { get { return _lunaMP; } }
    public int LunaMaxMP { get { return _lunaMaxMP; } }
    public int MonsterHP { get { return _monsterHP; } }
    public int MonsterMaxHP { get { return _monsterMaxHP; } }
    private GameManager() { }
    private void Awake()
    {
        Instance = this;
        _lunaMaxHP = 10;
        _lunaMaxMP = 10;
        _lunaMP = 5;
        _lunaHP = 8;
        _monsterHP = 4;
        _monsterMaxHP = 4;
    }
    public void ChangeLunaHP(int amount)
    {
        _lunaHP = Mathf.Clamp(_lunaHP + amount, 0, _lunaMaxHP);
        UIManager.Instance.SetHPValue((float)_lunaHP / _lunaMaxHP);
    }
    public void ChangeMonsterHP(int amount)
    {
        _monsterHP = Mathf.Clamp(_monsterHP + amount, 0, _monsterMaxHP);
        UIManager.Instance.SetMonsterHPValue((float)_monsterHP / _monsterMaxHP);
    }
    public void ChangeMP(int amount)
    {
        _lunaMP = Mathf.Clamp(_lunaMP + amount, 0, _lunaMaxMP);
        UIManager.Instance.SetMPValue((float)_lunaMP / _lunaMaxMP);
    }
    public bool DetectMP(int amount)
    {
        return _lunaMP < amount;
    }
    public void EnterOrExitBattle(bool enter, GameObject A, GameObject B)
    {
        BattleGround.SetActive(enter);
        PlayMusic(BattleClip);
        Luna = A;
        Monster = B;
        if (_monsterHP != 0)
        {
            Luna.SetActive(!enter);
            Monster.SetActive(!enter);
            StartCoroutine(_ignoreBattle());
        }
        else
        {
            Luna.SetActive(!enter);
        }
    }
    IEnumerator _ignoreBattle()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Monster"), true);
        int shine = 0;
        while (shine < 3)
        {
            Luna.GetComponentInChildren<SpriteRenderer>().DOFade(0.8f, 0.2f);
            yield return new WaitForSeconds(0.2f);
            Luna.GetComponentInChildren<SpriteRenderer>().DOFade(0.6f, 0.2f);
            yield return new WaitForSeconds(0.2f);
            Luna.GetComponentInChildren<SpriteRenderer>().DOFade(0.4f, 0.2f);
            yield return new WaitForSeconds(0.2f);
            Luna.GetComponentInChildren<SpriteRenderer>().DOFade(0.2f, 0.2f);
            yield return new WaitForSeconds(0.2f);
            Luna.GetComponentInChildren<SpriteRenderer>().DOFade(0.4f, 0.2f);
            yield return new WaitForSeconds(0.2f);
            Luna.GetComponentInChildren<SpriteRenderer>().DOFade(0.6f, 0.2f);
            yield return new WaitForSeconds(0.2f);
            Luna.GetComponentInChildren<SpriteRenderer>().DOFade(0.8f, 0.2f);
            yield return new WaitForSeconds(0.2f);
            Luna.GetComponentInChildren<SpriteRenderer>().DOFade(1, 0.2f);
            yield return new WaitForSeconds(0.2f);
            shine++;
        }
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Monster"), false);
    }
    public void ShowMonsters()
    {
        if (!Monsters.activeSelf)
        {
            Monsters.SetActive(true);
        }
    }
    public void PlayMusic(AudioClip AC)
    {
        if (AudioManager.clip != AC)
        {
            AudioManager.clip = AC;
            AudioManager.Play();
        }
    }
    public void PlaySound(AudioClip audioClip)
    {
        if (audioClip)
        {
            AudioManager.PlayOneShot(audioClip);
        }
    }
}
