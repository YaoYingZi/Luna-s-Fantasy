using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float[] _xNumbers = { -1, 0, 1 };
    public float Speed;
    public float ChangeTime;
    public GameObject BattleGround;
    private float _randomNumber;
    private int _direction = 1;
    private float _timer;
    private Rigidbody2D _rigidBody2D;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _randomNumber = _xNumbers[Random.Range(0, _xNumbers.Length)];
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        ChangeTime = Random.Range(0, 5);
        _timer = ChangeTime;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _randomNumber = _xNumbers[Random.Range(0, _xNumbers.Length)];
            _direction = -_direction;
            ChangeTime = Random.Range(0, 5);
            _timer = ChangeTime;
        }
    }
    private void FixedUpdate()
    {
        Vector3 pos = _rigidBody2D.position;
        _animator.SetFloat("LookX", _randomNumber);
        _animator.SetFloat("LookY", _direction);
        pos.x += Speed * _randomNumber * Time.fixedDeltaTime;
        pos.y += Speed * _direction * Time.fixedDeltaTime;
        //Debug.Log(_randomNumber + "," + _direction);
        _rigidBody2D.MovePosition(pos);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Luna") && BattleGround.activeSelf == false)
        {
            GameManager.Instance.EnterOrExitBattle(true, collision.gameObject, this.gameObject);
        }
    }
}
