using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInstance : MonoBehaviour
{
    public void PlaySoundEffect(AudioClip sfx)
    {
        
        SoundPlayer.instance.PlaySFX(sfx);
        
    }
}
