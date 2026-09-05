using Jan.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace Jan.Core
{
    public class SoundLibrary : Singleton<SoundLibrary>
    {
        [System.Serializable]
        private class Sound
        {
            [field: SerializeField, ValueDropdown(nameof(GetSoundNames))] public string Name { get; private set; }
            [field: SerializeField] public AudioClip[] Clips { get; private set; }

            private string[] GetSoundNames => GlobalsUtils.GetNames(typeof(SoundNames));
        }

        [BoxGroup("Audio Components")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField, BoxGroup("Audio Components")] private AudioSource[] audioSources;

        [SerializeField, Range(-80f, 20f), BoxGroup("Volumes")] private float masterVolume = 0f;
        [SerializeField, Range(-80f, 20f), BoxGroup("Volumes")] private float musicVolume = 0f;
        [SerializeField, Range(-80f, 20f), BoxGroup("Volumes")] private float sfxVolume = 0f;
        [SerializeField, Range(-80f, 20f), BoxGroup("Volumes")] private float uiVolume = 0f;


        [SerializeField, BoxGroup("Sounds")] private Sound[] UISounds;
        [SerializeField, BoxGroup("Sounds")] private Sound[] SFXSounds;
        [SerializeField, BoxGroup("Sounds")] private Sound[] MusicSounds;

        private AudioClip _previousClip;

        void Start()
        {
            SetVolume("Master", masterVolume);
            SetVolume("Music", musicVolume);
            SetVolume("SFX", sfxVolume);
            SetVolume("UI", uiVolume);
        }

        public static void PlayDelayedSound(string soundName, float delay)
        {
            Timed.CallDelayed(delay, () => PlaySound(soundName));
        }

        public static void PlaySound(string soundName, float volume = 1f)
        {
            // Search in UI sounds
            var uiSounds = Instance.UISounds;
            for(int i = 0; i < uiSounds.Length; i++)
            {
                if (uiSounds[i].Name.Equals(soundName))
                {
                    if(uiSounds[i].Clips.Length == 1) Instance._previousClip =  null;
                    var clipToPlay = uiSounds[i].Clips.RandomItemExcept(Instance._previousClip);
                    Instance.PlayClip(clipToPlay, "UI", volume);
                    Instance._previousClip = clipToPlay;
                    return;
                }
            }

            // Search in SFX sounds
            var sfxSounds = Instance.SFXSounds;
            for(int i = 0; i < sfxSounds.Length; i++)
            {
                if (sfxSounds[i].Name.Equals(soundName))
                {
                    if(sfxSounds[i].Clips.Length == 1) Instance._previousClip =  null;
                    var clipToPlay = sfxSounds[i].Clips.RandomItemExcept(Instance._previousClip);
                    Instance.PlayClip(clipToPlay, "SFX", volume);
                    Instance._previousClip = clipToPlay;
                    return;
                }
            }

            // Search in Music sounds
            var musicSounds = Instance.MusicSounds;
            for(int i = 0; i < musicSounds.Length; i++)
            {
                if (musicSounds[i].Name.Equals(soundName))
                {
                    if(musicSounds[i].Clips.Length == 1) Instance._previousClip =  null;
                    var clipToPlay = musicSounds[i].Clips.RandomItemExcept(Instance._previousClip);
                    Instance.PlayClip(clipToPlay, "Music", volume);
                    Instance._previousClip = clipToPlay;
                    return;
                }
            }

            Debug.LogWarning($"Sound '{soundName}' not found in any category.");
        }

        /// <summary>
        /// Starts a looping sound and returns the AudioSource playing it, so the caller can
        /// keep adjusting its volume and Stop it when done
        /// via StopLoopingSound.
        /// </summary>
        public static AudioSource PlayLoopingSound(string soundName, float volume = 0f)
        {
            var uiSounds = Instance.UISounds;
            for (int i = 0; i < uiSounds.Length; i++)
            {
                if (uiSounds[i].Name.Equals(soundName))
                    return Instance.PlayClip(uiSounds[i].Clips.RandomItem(), "UI", volume, true);
            }

            var sfxSounds = Instance.SFXSounds;
            for (int i = 0; i < sfxSounds.Length; i++)
            {
                if (sfxSounds[i].Name.Equals(soundName))
                    return Instance.PlayClip(sfxSounds[i].Clips.RandomItem(), "SFX", volume, true);
            }

            var musicSounds = Instance.MusicSounds;
            for (int i = 0; i < musicSounds.Length; i++)
            {
                if (musicSounds[i].Name.Equals(soundName))
                    return Instance.PlayClip(musicSounds[i].Clips.RandomItem(), "Music", volume, true);
            }

            Debug.LogWarning($"Sound '{soundName}' not found in any category.");
            return null;
        }

        public static void StopLoopingSound(AudioSource source)
        {
            if (source == null) return;

            source.loop = false;
            source.Stop();
        }

        public static void Play3DSound(string soundName, Vector3 position)
        {
            // Search in SFX sounds only for 3D sounds
            var sfxSounds = Instance.SFXSounds;
            for(int i = 0; i < sfxSounds.Length; i++)
            {
                if (sfxSounds[i].Name.Equals(soundName))
                {
                    AudioSource.PlayClipAtPoint(sfxSounds[i].Clips.RandomItem(), position, Instance.sfxVolume);
                    return;
                }
            }

            Debug.LogWarning($"3D Sound '{soundName}' not found in SFX category.");
        }

        private AudioSource PlayClip(AudioClip clip, string volumeParameter, float volume = 1f, bool loop = false)
        {
            if (clip == null)
            {
                Debug.LogWarning("AudioClip is null.");
                return null;
            }

            for (int i = 0; i < audioSources.Length; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    audioSources[i].clip = clip;
                    audioSources[i].volume = volume;
                    audioSources[i].loop = loop;
                    audioSources[i].outputAudioMixerGroup = audioMixer.FindMatchingGroups(volumeParameter)[0];
                    audioSources[i].Play();
                    return audioSources[i];
                }
            }

            Debug.LogWarning("All audio sources are currently playing. Consider increasing the number of audio sources.");
            return null;
        }

        public static void SetVolume(string parameterName, float volume)
        {
            
            if (Instance.audioMixer == null)
            {
                Debug.LogWarning("SoundLibrary not initialized with an AudioMixer.");
                return;
            }

            Instance.audioMixer.SetFloat(parameterName, volume);
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if(Instance == null) return;
            SetVolume("Master", masterVolume);
            SetVolume("Music", musicVolume);
            SetVolume("SFX", sfxVolume);
            SetVolume("UI", uiVolume);
        }
#endif
    }
}
