using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Dog : MonoBehaviour
{
    private Animator _animator;
    public GameObject StarsEffect;
    public AudioClip Pet;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(GameManager.Instance.DialogInfoIndex == 2)
        {
            StarsEffect.SetActive(true);
        }
    }
    public void BeHappy()
    {
        _animator.CrossFade("Comforted", 0);
        GameManager.Instance.PlaySound(Pet);
        GetComponent<AudioSource>().Stop();
        GameManager.Instance.HasPetTheDog = true;
        GameManager.Instance.DialogInfoIndex ++;
        Destroy(StarsEffect);
        Invoke("CanControlLuna", 1.75f);
    }

    private void CanControlLuna()
    {
        GameManager.Instance.CanControlLuna = true;
    }
}
