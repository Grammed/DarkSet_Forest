using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private SO_MainGun mainGun;

    [SerializeField] private AudioSource audio;
    
    public static SoundManager _instance;
    
    public void SetGunFireVolume(float volume)
    {
        audio.volume = volume;
    }
    
    private void Awake()
    {
        //gunSoundPool = FindAnyObjectByType<GunSoundPool>();
        
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
