using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class LunaController : MonoBehaviour
{
    public float MoveSpeed;
    private Animator _animator;
    private Rigidbody2D _rigidBody2d;
    private float _moveScale;
    private Vector2 move;
    private Vector2 _lookDirection = new(1, 0);
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ExitGame = true;
        _rigidBody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        //Application.targetFrameRate = xxx; 设置画面每秒更新帧率
    }

    // Update is called once per frame
    void Update()
    {
        ////规范空格ctrl k + ctrl d
        ////多行注释ctrl k + ctrl c 取消ctrl k + ctrl u
        ////获取玩家水平轴向输入值
        //float horizontal = Input.GetAxis("Horizontal");
        ////获取玩家垂直轴向输入值
        //float vertical = Input.GetAxis("Vertical");
        //Vector2 position = transform.position;
        //position.x = position.x + moveSpeed * horizontal * Time.deltaTime;
        //position.y = position.y + moveSpeed * vertical * Time.deltaTime;
        ////transform.position = position;               
        if (GameManager.Instance.CandleNum >= 5 && GameManager.Instance.DialogInfoIndex == 4)
        {
            GameManager.Instance.DialogInfoIndex = 5;
        }
        if (GameManager.Instance.KillNum == 5 && GameManager.Instance.DialogInfoIndex == 6)
        {
            GameManager.Instance.DialogInfoIndex = 7;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            Talk();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.ExitPanel.SetActive(GameManager.Instance.ExitGame);
            GameManager.Instance.ExitGame = !GameManager.Instance.ExitGame;
        }
    }
    void FixedUpdate()
    {
        if (!GameManager.Instance.CanControlLuna)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Monster"), true);
            _animator.SetFloat("MoveValue", 0);
            return;
        }
        //position.x = position.x + moveSpeed * horizontal * Time.deltaTime;
        //position.y = position.y + moveSpeed * vertical * Time.deltaTime;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        move = new(horizontal, vertical);
        Vector2 position = _rigidBody2d.position;
        //Debug.Log(horizontal + "," + vertical);
        //position.x = position.x + MoveSpeed * horizontal * Time.fixedDeltaTime;
        //position.y = position.y + MoveSpeed * vertical * Time.fixedDeltaTime;


        //当前玩家输入的某个轴向不为0,无法准确同时为0使得idle斜向触发不太稳定，这个问题跟帧率也有点关系，在fixed里面会更加稳定
        if (!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0))
        {
            _lookDirection.Set(move.x, move.y);
            //Debug.Log(move.x + "," + move.y);
            //_lookDirection = move;
            _lookDirection.Normalize();
        }
        // if (!Mathf.Equals(0, move.x) || !Mathf.Equals(0, move.y)) //这样写0,0都能进入if语句!
        _animator.SetFloat("LookX", _lookDirection.x);
        _animator.SetFloat("LookY", _lookDirection.y);
        //Debug.Log("move.magnitude"+move.magnitude);
        _moveScale = move.magnitude;
        MoveSpeed = 1;
        if (move.magnitude > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            _moveScale = 2;
            MoveSpeed = 2.5f;
        }
        //Debug.Log("_moveScale" + _moveScale);
        position = position + MoveSpeed * move.normalized * Time.fixedDeltaTime;
        _animator.SetFloat("MoveValue", _moveScale);
        _rigidBody2d.MovePosition(position);
    }
    public void Climb(bool climb)
    {
        _animator.SetBool("Climb", climb);

    }
    public void ClimbPause()
    {
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)
         && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            _animator.speed = 0;
        }
        else
        {
            _animator.speed = 1;
        }
    }
    public void Jump(bool jump)
    {
        _animator.SetBool("Jump", jump);
        _rigidBody2d.simulated = !jump;
    }
    private void PetTheDog()
    {
        _animator.CrossFade("PetTheDog", 0);
        transform.position = new Vector3(-1.81f, -7.43f, 0);
    }
    public void Talk()
    {
        Collider2D collider = Physics2D.OverlapCircle(_rigidBody2d.position,
            0.5f, LayerMask.GetMask("NPC"));
        if (collider != null)
        {
            if (collider.name == "Nala")
            {
                GameManager.Instance.CanControlLuna = false;
                collider.GetComponent<NPCDialogue>().DisplayDialog();
            }
            else if (collider.name == "Dog"
                && !GameManager.Instance.HasPetTheDog &&
                GameManager.Instance.DialogInfoIndex == 2)
            {
                PetTheDog();
                GameManager.Instance.CanControlLuna = false;
                collider.GetComponent<Dog>().BeHappy();
            }
        }
    }
}
