using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInstance : MonoBehaviour
{
    public void PlaySoundEffect(AudioClip sfx)
    {

        if (sfx != null)
        {
            
            SoundPlayer.instance.PlaySFX(sfx);   
            
        }

    }
}
