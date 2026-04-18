using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float DestoryTime;
    // Start is called before the first frame update
    void Start()
    {
        if (DestoryTime != -1)
        {
            Destroy(gameObject, DestoryTime);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
