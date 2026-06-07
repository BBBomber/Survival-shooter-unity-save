using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MixLevels : MonoBehaviour
{
    public AudioMixer masterMixer;   

    public void SetMusicLvl(float musicLvl)
    {
        if (ConfigManager.Instance != null) ConfigManager.Instance.SetMusic(musicLvl);
        else if (masterMixer != null) masterMixer.SetFloat("musicVol", musicLvl);
    }

    public void SetSfxLvl(float sfxLvl)
    {
        if (ConfigManager.Instance != null) ConfigManager.Instance.SetEffects(sfxLvl);
        else if (masterMixer != null) masterMixer.SetFloat("sfxVol", sfxLvl);
    }
}