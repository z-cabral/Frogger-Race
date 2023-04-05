using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBehavior : MonoBehaviour
{
    public static AudioBehavior Instance = null;

    [SerializeField] AudioSource EffectsSource, MusicSource;

    [SerializeField] AudioClip MusicClip;

    bool isplaying;

    [SerializeField] float LowPitchRange = .95f;
    [SerializeField] float HighPitchRange = 1.05f;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(GameBehavior.Instance.CurrentState == State.Play && isplaying == false)
        {
            PlayMx(MusicClip);
        }
        if (GameBehavior.Instance.CurrentState == State.GameOver && isplaying == true)
        {
            StopMx();
        }

    }

    private void PlayMx(AudioClip Clip)
    {
        MusicSource.clip = Clip;
        MusicSource.loop = true;
        MusicSource.Play();

        isplaying = true;
    }

    private void StopMx()
    {
        MusicSource.Stop();

        isplaying = false;
    }

    public void RandomSFX(AudioClip clip)
    {
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

        EffectsSource.pitch = randomPitch;
        EffectsSource.clip = clip;
        EffectsSource.Play();
    }
}
