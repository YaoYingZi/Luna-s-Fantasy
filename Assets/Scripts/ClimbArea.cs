using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Luna"))
        {
            collider.GetComponent<LunaController>().Climb(true);   
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Luna"))
        {
            collider.GetComponent<LunaController>().Climb(false);
        }
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Luna"))
        {
            collider.GetComponent<LunaController>().ClimbPause();
        }
    }
}
