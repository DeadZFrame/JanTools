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

        [SerializeField, Range(0f, 1f), BoxGroup("Volumes")] private float masterVolume = 1f;
        [SerializeField, Range(0f, 1f), BoxGroup("Volumes")] private float musicVolume = 1f;
        [SerializeField, Range(0f, 1f), BoxGroup("Volumes")] private float sfxVolume = 1f;
        [SerializeField, Range(0f, 1f), BoxGroup("Volumes")] private float uiVolume = 1f;


        [SerializeField, BoxGroup("Sounds")] private Sound[] UISounds;
        [SerializeField, BoxGroup("Sounds")] private Sound[] SFXSounds;
        [SerializeField, BoxGroup("Sounds")] private Sound[] MusicSounds;

        void Start()
        {
            SetVolume("Master", masterVolume);
            SetVolume("Music", musicVolume);
            SetVolume("SFX", sfxVolume);
            SetVolume("UI", uiVolume);
        }

        public static void PlaySound(string soundName)
        {
            // Search in UI sounds
            var uiSounds = Instance.UISounds;
            for(int i = 0; i < uiSounds.Length; i++)
            {
                if (uiSounds[i].Name.Equals(soundName))
                {
                    Instance.PlayClip(uiSounds[i].Clips.RandomItem(), "UI");
                    return;
                }
            }

            // Search in SFX sounds
            var sfxSounds = Instance.SFXSounds;
            for(int i = 0; i < sfxSounds.Length; i++)
            {
                if (sfxSounds[i].Name.Equals(soundName))
                {
                    Instance.PlayClip(sfxSounds[i].Clips.RandomItem(), "SFX");
                    return;
                }
            }

            // Search in Music sounds
            var musicSounds = Instance.MusicSounds;
            for(int i = 0; i < musicSounds.Length; i++)
            {
                if (musicSounds[i].Name.Equals(soundName))
                {
                    Instance.PlayClip(musicSounds[i].Clips.RandomItem(), "Music");
                    return;
                }
            }

            Debug.LogWarning($"Sound '{soundName}' not found in any category.");
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

        private void PlayClip(AudioClip clip, string volumeParameter)
        {
            if (clip == null)
            {
                Debug.LogWarning("AudioClip is null.");
                return;
            }

            for (int i = 0; i < audioSources.Length; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    audioSources[i].clip = clip;
                    audioSources[i].outputAudioMixerGroup = audioMixer.FindMatchingGroups(volumeParameter)[0];
                    audioSources[i].Play();
                    return;
                }
            }

            Debug.LogWarning("All audio sources are currently playing. Consider increasing the number of audio sources.");
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
    }
}
