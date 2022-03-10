using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ControllerIntro : MonoBehaviour
{
    public VideoPlayer VPlayer;

    public VideoClip IntroVideo;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("score", 0);
        VPlayer.clip = IntroVideo;
        VPlayer.Play();
        VPlayer.loopPointReached += CheckVideoOver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckVideoOver(VideoPlayer vp)
    {
        SceneManager.LoadScene("Menu");
    }

}
