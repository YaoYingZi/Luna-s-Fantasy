using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public GameObject Effect;
    public AudioClip HealSound;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Luna"))
        {
            if (GameManager.Instance.LunaHP < GameManager.Instance.LunaMaxHP || GameManager.Instance.LunaMP < GameManager.Instance.LunaMaxMP)
            {
                GameManager.Instance.ChangeLunaHP(1);
				GameManager.Instance.ChangeMP(2);
                GameManager.Instance.PlaySound(HealSound);
				Instantiate(Effect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
