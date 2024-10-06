using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public AudioSource musicSource;
        public AudioClip musicClip;
        
        [Range(0f, 1f)]
        public float musicVolume = 1f;
        [Range(0f, 1f)]
        public float sfxVolume = 1f;
            
        private List<AudioSource> _sfxSources = new();
        public int maxSfxSources = 5;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            // Create a pool of AudioSources for SFX
            for (var i = 0; i < maxSfxSources; i++)
            {
                AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.volume = sfxVolume;
                _sfxSources.Add(sfxSource);
            }
        }

        private void Start()
        {
            PlayMusic(musicClip);
        }
        
        public void PlayMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }

        private AudioSource GetAvailableSfxSource()
        {
            foreach (var source in _sfxSources)
            {
                if (!source.isPlaying)
                {
                    source.volume = sfxVolume;
                    return source;
                }
            }
            return null;
        }

        public void PlaySfx(AudioClip clip)
        {
            AudioSource source = GetAvailableSfxSource();
            if (source != null)
            {
                source.PlayOneShot(clip);
            }
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = volume;
            musicSource.volume = volume;
        }

        public void ToggleMute()
        {
            bool isMuted = AudioListener.pause;
            AudioListener.pause = !isMuted;
        }
    }
}