using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum Songs
{
    
    ADRENALINE,
    GameplaySong
    
}

public enum SoundEffects
{
    
    MenuButtonPress,
    JoinGame,
    
}

public enum TransitionBehavior
{
    
    Stop,Pause,Continue
    
}

public class SoundPlayer : MonoBehaviour
{

    public static SoundPlayer instance;
    
    private AudioSource source;
    public AudioSource source1;
    public AudioSource source2;

    static Dictionary<Songs, AudioClip> songs;
    static Dictionary<SoundEffects, AudioClip> effects;
    
    [Range(0,1)]
    public float globalVolume; // global volume multiplier

    // singleton initializer
    private void Awake()
    {
        
        //singleton
        if (instance is null) instance = this;
        else Destroy(gameObject);

        InitializeSounds();

        SetGlobalVolume(globalVolume);
        
        source = source1;
        
        //test
        //StartCoroutine(testMusicCor());

    }

    public void SetGlobalVolume(float value)
    {

        globalVolume = value;
        AudioListener.volume = value;

    }
    
    IEnumerator testMusicCor()
    {
        yield return new WaitForSeconds(5f);
        SetMusic(Songs.GameplaySong,2.5f, TransitionBehavior.Continue); //start wdib
        yield return new WaitForSeconds(5f);
        SetMusic(Songs.ADRENALINE, 2.5f, TransitionBehavior.Pause); // come back to continued adrenaline
        yield return new WaitForSeconds(5f);
        SetMusic(Songs.GameplaySong, 2.5f, TransitionBehavior.Stop); //start from last place in wdib
        yield return new WaitForSeconds(5f);
        SetMusic(Songs.ADRENALINE, 2.5f, TransitionBehavior.Stop);//new start of adrenaline

    }

    /// <summary>
    /// Starts song with no transition
    /// </summary>
    /// <param name="song"></param>
    /// <param name="startPlaying"></param>
    public void SetMusic(Songs song, bool startPlaying = true)
    {

        if (songs.ContainsKey(song))
        {

            if (source.isPlaying) {source.Stop();}
            source.clip = songs[song];
            if (startPlaying) {source.Play();}

        }

    }
    
    /// <summary>
    /// Starts song with smooth transition between songs
    /// </summary>
    /// <param name="song"></param>
    /// <param name="transitionTime"></param>
    public void SetMusic(Songs song, float transitionTime, TransitionBehavior behavior)
    {

        if (songs.ContainsKey(song))
        {

            StartCoroutine(ChangeMusicTransition(song, transitionTime, behavior));

        }
        else
        {
            
            Debug.LogError($"Error: Song {song.ToString()} not found in songs dictionary. (SoundPlayer.cs, line 105)");
            
        }

    }

    private IEnumerator ChangeMusicTransition(Songs song, float transitionTime, TransitionBehavior behavior)
    {

        AudioSource sourceA, sourceB;

        //sets sourceA as source thats playing and B as source thats gonna be playing
        sourceA = source;
        sourceB = source == source1 ? source2 : source1;

        //set clip
        sourceB.clip = songs[song];
        sourceB.volume = 0;
        sourceB.UnPause();
        if (!sourceB.isPlaying) sourceB.Play();
        
        while (sourceB.volume < 1)
        {

            float delta = Time.deltaTime/transitionTime;
            sourceA.volume = Mathf.Max(0, sourceA.volume - delta);
            sourceB.volume = Mathf.Min(1, sourceB.volume + delta);
            yield return null;
            
        }

        switch (behavior)
        {
            
            case TransitionBehavior.Stop: sourceA.Stop(); break;
            case TransitionBehavior.Pause: sourceA.Pause(); break;
            
        }
        
        
        source = sourceB;


    }

    public void PlaySFX(SoundEffects sfx, float vol = 1f)
    {

        if (effects.ContainsKey(sfx))
        {
            
            source.PlayOneShot(effects[sfx],2*globalVolume*vol);   
            
        }

    }

    void InitializeSounds()
    {
        
        //put songs in this list
        songs = new Dictionary<Songs, AudioClip>()
        {
            {Songs.ADRENALINE, Resources.Load<AudioClip>("Sound/Music/ADRENALINE")},
            {Songs.GameplaySong, Resources.Load<AudioClip>("Sound/Music/Where Do I Belong")},
            //...
        };

        //put sfx in this list
        effects = new Dictionary<SoundEffects, AudioClip>()
        {

            {SoundEffects.MenuButtonPress, Resources.Load<AudioClip>("Music/Forest")},
            {SoundEffects.JoinGame, Resources.Load<AudioClip>("Music/Forest")},
            //...

        };
        
    }
    
}
