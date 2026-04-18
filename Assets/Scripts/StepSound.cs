using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSound : MonoBehaviour
{
    public AudioClip Step;
    public AudioSource AudioStep;
    public float stepInterval = 0.3f; 
    private float _nextPlayTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayStepSound(AudioClip AC)
    {
        if (Time.time < _nextPlayTime)
            return;

        _nextPlayTime = Time.time + stepInterval;

        AudioStep.PlayOneShot(AC);
    }
}
