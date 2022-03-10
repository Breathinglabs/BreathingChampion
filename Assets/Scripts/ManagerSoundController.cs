using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ManagerSoundController : MonoBehaviour
{

    public AudioSource Audio;

    public AudioSource AudioEffects;


    public static ManagerSoundController instance;

    public bool IsMute { get; set; }

    
     
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);


    }



    // Start is called before the first frame update
    void Start()
    {
        StartSound();



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Muted()
    {
        DataController.playerData.IsMute = !DataController.playerData.IsMute;
        Audio.mute = DataController.playerData.IsMute;
        AudioEffects.mute = DataController.playerData.IsMute;


    }

    public void StartSound()
    {
        Audio = GetComponent<AudioSource>();


        Audio.mute = DataController.playerData.IsMute;
        AudioEffects.mute = DataController.playerData.IsMute;


    }

    public void PlayAudioEffects(AudioClip audioClip) {

        AudioEffects.clip = audioClip;
        AudioEffects.time = 0.001f;
        AudioEffects.Play();
    } 


    public void PauseAudio()
    {
        Audio.Pause();
        AudioEffects.Pause();
    }

    public void StopAudio()
    {
        Audio.Stop();

        AudioEffects.clip = null;
        AudioEffects.Stop();
    }

    public void PlayAudio()
    {
        Audio.Play();
        AudioEffects.Play();
    }

    public void TimeAduio(double time)
    {

        AudioEffects.time = (float)time;
        Audio.Pause();
    }

    public void PlayMusic(bool isOn)
    {
        if(isOn) Audio.Play();
        else Audio.Pause();
    }

    public void RestartAudio()
    {

    }
}
