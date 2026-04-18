using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    public GameObject Effect;
    public AudioClip PickSound;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Luna"))
        {
            Instantiate(Effect, transform.position, Quaternion.identity);
            GameManager.Instance.PlaySound(PickSound);
            Destroy(gameObject);
            GameManager.Instance.CandleNum++;
        }
    }
}
