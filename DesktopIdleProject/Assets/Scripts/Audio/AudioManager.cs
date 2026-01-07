using System;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //[SerializeField] Sound[] musics;
    [SerializeField] Sound[] effects;

    //[Space(10)]
    //[SerializeField] float thresholdCurrentMusic = 0.7f;

    //private List<Sound> bgMusics;
    //private int bgMusicIndex = -1;
    //private int bgMusicNextIndex = -1;

    //private bool isChangingBGMusicAuto;

    //private bool isChanging;

    //private float musicPlayingFor;

    //private bool hasForcedStop;

    //public bool HasForcedStop => hasForcedStop;

    [Space(10)]
    [SerializeField] AudioMixer mixer;

    //[Space(10)]
    //[SerializeField] float changeMusicTime = 2f;


    //public float ChangeMusicTime => changeMusicTime;



    //private int currentPlayingIndex = -1;
    //private int nextPlayingIndex = -1;

    // change music variable
    //private string nextMusic;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        //bgMusics = new List<Sound>();
        /*
        foreach (var sound in musics)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = sound.group;

            if(sound.tag == "BG")
            {
                bgMusics.Add(sound);
            }
        }
        */
        foreach (var sound in effects)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = sound.group;
        }
    }
    /*
    private void Update()
    {
        // update timer for music
        musicPlayingFor += Time.deltaTime;

        if (isChangingBGMusicAuto && !isChanging)
        {
            // check if music is playing for at least 95% of it
            if (musicPlayingFor >= bgMusics[bgMusicIndex].source.clip.length * thresholdCurrentMusic)
            {
                ChangeAutomaticallyBGMusic();
            }
        }
    }
    */
    /*
    private IEnumerator CoLoweringMusic()
    {
        float elapsed = 0;
        float startVol = 1f;

        if(currentPlayingIndex >= 0)
            startVol = musics[currentPlayingIndex].source.volume;

        while (elapsed < changeMusicTime / 2)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / changeMusicTime / 2;

            if (currentPlayingIndex >= 0)
            {
                musics[currentPlayingIndex].source.volume = Mathf.Lerp(startVol, 0f, t);
            }

            yield return null;
        }

        if (currentPlayingIndex >= 0)
            musics[currentPlayingIndex].source.volume = 0;
        
        StopMusic();

        bgMusicIndex = bgMusicNextIndex;

        PlayMusic(nextMusic);

        StartCoroutine(CoIncreasingMusic());
    }
    */
    /*
    private IEnumerator CoIncreasingMusic()
    {
        float elapsed = 0;
        float endVol = musics[currentPlayingIndex].volume;
        while (elapsed < changeMusicTime / 2)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / changeMusicTime / 2;
            musics[currentPlayingIndex].source.volume = Mathf.Lerp(0f, endVol, t);

            yield return null;
        }

        isChanging = false;
    }
    */

    public void PlayEffect(string name)
    {
        Sound s = Array.Find(effects, sound => sound.name == name);

        if (s == null)
            return;
        
        s.source.Play();
    }
    /*
    public void PlayMusic(string name)
    {
        Sound s = null;
        int index = -1;

        for (int i = 0; i < musics.Length; i++)
        {
            if (musics[i].name == name)
            {
                s = musics[i];
                index = i;
            }
        }

        if (s == null)
            return;

        currentPlayingIndex = index;
        //s.source.volume = s.volume;
        s.source.volume = 0;
        s.source.Play();
        musicPlayingFor = 0;
    }
    */
    /*
    public void ChangeAutomaticallyBGMusic(string selectedMusic = "")
    {
        // reset
        hasForcedStop = false;

        isChanging = true;
        isChangingBGMusicAuto = true;
        int rand;

        string nextName;

        if (selectedMusic == "")
        {
            do
            {
                rand = UnityEngine.Random.Range(0, bgMusics.Count);

            } while (rand == bgMusicIndex && bgMusics.Count > 1);

            bgMusicNextIndex = rand;

            nextName = bgMusics[bgMusicNextIndex].name;
        }
        else
        {
            for (int i = 0; i < bgMusics.Count; i++)
            {
                if (bgMusics[i].name == selectedMusic)
                {
                    bgMusicNextIndex = i;
                }
            }

            nextName = selectedMusic;
        }

        for (int i = 0; i < musics.Length; i++)
        {
            if (musics[i].name == nextName)
            {
                nextPlayingIndex = i;
            }
        }

        ChangeMusic(nextName);
    }
    */
   /*

    public void ChangeMusic(string name)
    {
        nextMusic = name;

        StartCoroutine(CoLoweringMusic());
    }
   */
   /*
    public void StopMusic()
    {
        if (currentPlayingIndex >= 0)
            musics[currentPlayingIndex].source.Stop();

        currentPlayingIndex = -1;
        hasForcedStop = true;
    }
   */
   /*
    public IEnumerator CoStopMusic(float timer)
    {
        isChangingBGMusicAuto = false;

        float elapsed = 0;
        float startVol = 1f;

        if (currentPlayingIndex >= 0)
            startVol = musics[currentPlayingIndex].source.volume;

        while (elapsed < timer)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = elapsed / timer;

            if (currentPlayingIndex >= 0)
            {
                musics[currentPlayingIndex].source.volume = Mathf.Lerp(startVol, 0f, t);
            }

            yield return null;
        }

        if (currentPlayingIndex >= 0)
        {
            musics[currentPlayingIndex].source.volume = 0;
        }

        StopMusic();
    }
   */
   /*
    public void ResumeMusic()
    {
        if (currentPlayingIndex >= 0)
            musics[currentPlayingIndex].source.Play();
    }

    public void ChangeMusicVolume(float value)
    {
        if (currentPlayingIndex >= 0)
            musics[currentPlayingIndex].source.volume = value;
    }

    public string GetMusicPlayingName()
    {
        if (currentPlayingIndex >= 0)
            return musics[currentPlayingIndex].name;
        return string.Empty;
    }

    */
    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
    /*
    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetEffectsVolume(float volume)
    {
        mixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
    }*/
}
