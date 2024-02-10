using System;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    public enum SoundTag
    {
        PerfectMatched,
    }

    [Header("Config")]
    public SoundsWrapper[] sounds;
    const float _pitchRange = 2.9f;
    float step;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < sounds.Length; i++)
        {
            SoundsWrapper wrapper = sounds[i];
            wrapper.audioSource = gameObject.AddComponent<AudioSource>();
            wrapper.audioSource.clip = wrapper.clip;
            wrapper.audioSource.volume = wrapper.volume;
            wrapper.audioSource.pitch = wrapper.pitch;
            wrapper.SetInitPitch();
        }
    }

    private void Start()
    {
        GameManager.instance.LevelEndedEvent += OnLevelEnded;
        float maxBlock = BlockSpawnManager.instance.GetMaxBlockCount();
        step = _pitchRange / maxBlock;

    }

    private void OnLevelEnded()
    {
        OnComboEnded(SoundTag.PerfectMatched);
    }

    public void PlaySoundEffect(SoundTag tag, bool manipulatePitch = false)
    {
        SoundsWrapper soundWrapper = GetWrapper(tag);
        AudioSource source = soundWrapper.audioSource;

        if (manipulatePitch)
        {
            soundWrapper.pitch += step;
            source.pitch += step;
        }

        if (source != null)
        {
            if (!source.isPlaying)
                source.Play();
        }
    }

    public void OnComboEnded(SoundTag tag)
    {
        SoundsWrapper soundWrapper = GetWrapper(tag);
        soundWrapper.ResetPitch();
    }


    SoundsWrapper GetWrapper(SoundTag tag)
    {
        SoundsWrapper sw = null;
        foreach (SoundsWrapper sound in sounds)
        {
            if (sound.tag == tag)
                sw = sound;
        }
        return sw;
    }
}



[Serializable]
public class SoundsWrapper
{
    public AudioClip clip;
    public AudioManager.SoundTag tag;
    [HideInInspector] public AudioSource audioSource;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;
    public float initialPitch;

    public void ResetPitch()
    {
        pitch = initialPitch;
        audioSource.pitch = pitch;
    }

    public void SetInitPitch()
    {
        initialPitch = pitch;
    }
}
