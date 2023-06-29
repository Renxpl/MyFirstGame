using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

public class AudioManagerScript : MonoBehaviour
{

    public Sound[] sounds;
    public bool isStopped;

    public static AudioManagerScript instance { get; private set; }
    void Awake()
    {

        // if (instance != null) { Destroy(gameObject); }
        //instance = this;
        //DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audio;
            s.source.volume = s.volume;

        }
    }
    public void Play(string name, bool isLoop)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        if (!s.source.isPlaying)
        {
            s.source.PlayDelayed(s.delaySeconds);
            s.source.loop = isLoop;
        }
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;

        s.source.Stop();
        

    }
    void Start()
    {
        

        Play("MenuMusic", true);
        

    }

    
    void Update()
    {
        if (!isStopped && SceneManager.GetActiveScene().buildIndex == 0)
        {
            isStopped= true;
            Stop("MainMusic");
            Sound foundAudioSource = Array.Find(sounds, sound => sound.name == "MenuMusic");
            if (!foundAudioSource.source.isPlaying)
            {
                Play("MenuMusic", true);
            }
        }

        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            isStopped= false;
            Stop("MenuMusic");
        }



    }
}
