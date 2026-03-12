using System;
using System.Collections.Generic;
using Jan.Core;
using Jan.Events;
using Jan.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Giant.Feel
{
    [Serializable]
    public class Feedback
    {
        [ValueDropdown(nameof(GetEventNames), DropdownHeight = 200), SerializeField, Space]
        [Tooltip("Event name or identifier for this feedback. Use EventTypes or custom names to trigger this feedback.")]
        public string name = "Feedback";

        [ValueDropdown(nameof(GetOptions)), SerializeField, OnValueChanged(nameof(OptionSelected)), Space]
        [Tooltip("Select a feedback type to add to this Feedback entry (Sound, VFX, Shake, etc.).")]
        private string selectedOption = "Add Feedback";

        [Tooltip("List of sound feedbacks to play when this feedback is triggered.")]
        [SerializeField, ShowIf(nameof(ShowSounds)), FoldoutGroup("Feedbacks", true)]
        private List<Sound> sounds = new List<Sound>();
        [Tooltip("Particle/visual effects to spawn when this feedback is triggered.")]
        [Space, SerializeField, ShowIf(nameof(ShowEffects)), FoldoutGroup("Feedbacks", true)]
        private List<VFX> effects = new List<VFX>();
        [Tooltip("Object shake feedbacks to apply to target transforms when triggered.")]
        [SerializeField, ShowIf(nameof(ShowShakes)), FoldoutGroup("Feedbacks", true)]
        private List<ObjectShake> shakes = new List<ObjectShake>();
        [Tooltip("Scale punch/stretch feedbacks applied to target transforms when triggered.")]
        [SerializeField, ShowIf(nameof(ShowSquashStretches)), FoldoutGroup("Feedbacks", true)]
        private List<SquashStretch> squash = new List<SquashStretch>();
        [Tooltip("Optional color gradient feedbacks to apply to target materials when triggered.")]
        [SerializeField, ShowIf(nameof(ShowCameraZooms)), FoldoutGroup("Feedbacks", true)]
        private List<CameraZoom> cameraZooms = new List<CameraZoom>();
        [Tooltip("Color gradient feedbacks to apply to target materials when triggered.")]
        [SerializeField, ShowIf(nameof(ShowColorGradients)), FoldoutGroup("Feedbacks", true)]        
        private List<ColorGradient> colorGradients = new List<ColorGradient>();
        [Tooltip("Object movement feedbacks to apply when this feedback is triggered.")]
        [SerializeField, ShowIf(nameof(ShowMovements)), FoldoutGroup("Feedbacks", true)]
        private List<ObjectMove> movements = new List<ObjectMove>();
        private bool ShowSounds => sounds.Count > 0;
        private bool ShowEffects => effects.Count > 0;
        private bool ShowShakes => shakes.Count > 0;
        private bool ShowSquashStretches => squash.Count > 0;
        private bool ShowColorGradients => colorGradients.Count > 0;
        private bool ShowCameraZooms => cameraZooms.Count > 0;
        private bool ShowMovements => movements.Count > 0;
        private bool IsAppPlaying => Application.isPlaying;

        public Transform feedbackTransform;
        public bool listenEvent;
        private string[] GetEventNames => GlobalsUtils.GetNames(typeof(EventNames));

        [Button, ShowIf(nameof(IsAppPlaying)), GUIColor(1f, .5f, .5f)]
        public void Play(Transform transform = null, Vector3 position = default, Quaternion rotation = default)
        {
            transform ??= feedbackTransform;

            sounds?.ForEach(s => Play(s, transform));
            shakes?.ForEach(sh => Play(sh, transform));
            squash?.ForEach(sq => Play(sq, transform));
            cameraZooms?.ForEach(cz => Play(cz, transform));
            colorGradients?.ForEach(cg => Play(cg, transform));
            movements?.ForEach(e => Play(e, transform));

            effects?.ForEach(e =>
            {
                var pos = transform != null ? transform.position : position;
                pos = position != default ? position : pos;
                var rot = transform != null ? transform.rotation : rotation;
                rot = rotation != default ? rotation : rot;

                // void SpawnVfx()
                // {
                //     var effect = LeanPool.Spawn(e.VFXAgent, pos, rot);
                //     var lifeTime = effect.GetLongestLifeTime();
                //     var time = lifeTime > 0 ? lifeTime : e.Duration;
                //     effect.Play();
                //     if (time > 0)
                //     {
                //         effect.CallDelayed(time, LeanPool.Despawn, transform.gameObject).OnCancelled(() => LeanPool.Despawn(effect));
                //     }
                // }

                // if (e.Delay > 0) Timed.CallDelayed(e.Delay, SpawnVfx, transform);
                // else SpawnVfx();
            });
        }


        private void Play(FeedbackBase feedback, Transform transform)
        {
            if (feedback.Delay > 0) Timed.CallDelayed(feedback.Delay, () => feedback.Play(transform));
            else feedback.Play(transform);
        }

        public void Play() => Play(null);

        //TODO: Reference in prefabs, do not use GetComponent()
        public void Initialize(Transform transform)
        {
            feedbackTransform ??= transform;
        }

        private string[] GetOptions => new[] { "Sound", "Vfx", "Shake", "Stretch & Squash", "Camera Zoom", "Color", "Move" };
        private void OptionSelected()
        {
            switch (selectedOption)
            {
                case "Sound": sounds.Add(new Sound()); break;
                case "Vfx": effects.Add(new VFX()); break;
                case "Shake": shakes.Add(new ObjectShake()); break;
                case "Stretch & Squash": squash.Add(new SquashStretch()); break;
                case "Camera Zoom": cameraZooms.Add(new CameraZoom()); break;
                case "Color": colorGradients.Add(new ColorGradient()); break;
                case "Move": movements.Add(new ObjectMove()); break;
            }

            selectedOption = "Add Feedback";
        }

        internal T GetFeedback<T>(int index) where T : FeedbackBase
        {
            FeedbackBase feedback = typeof(T) switch
            {
                Type t when t == typeof(Sound) && index >= 0 && index < sounds.Count => sounds[index],
                Type t when t == typeof(VFX) && index >= 0 && index < effects.Count => effects[index],
                Type t when t == typeof(ObjectShake) && index >= 0 && index < shakes.Count => shakes[index],
                Type t when t == typeof(SquashStretch) && index >= 0 && index < squash.Count => squash[index],
                Type t when t == typeof(ColorGradient) && index >= 0 && index < colorGradients.Count => colorGradients[index],
                Type t when t == typeof(CameraZoom) && index >= 0 && index < cameraZooms.Count => cameraZooms[index],
                Type t when t == typeof(ObjectMove) && index >= 0 && index < movements.Count => movements[index],
                _ => null
            };

            return feedback as T;
        }

        internal FeedbackBase[] GetFeedbacks()
        {
            var feedbacks = new List<FeedbackBase>();
            feedbacks.AddRange(sounds);
            feedbacks.AddRange(effects);
            feedbacks.AddRange(shakes);
            feedbacks.AddRange(squash);
            feedbacks.AddRange(colorGradients);
            feedbacks.AddRange(cameraZooms);
            feedbacks.AddRange(movements);

            return feedbacks.ToArray();
        }

        public float GetFeedbackDuration()
        {
            float duration = 0f;
            foreach (var feedback in GetFeedbacks())
            {
                if (feedback != null && feedback.Duration > duration)
                {
                    duration = feedback.Duration + feedback.Delay;
                }
            }

            return duration;
        }

        public void OnComplete(Action callback, GameObject cullingObject = null)
        {
            // Call the callback action when all feedbacks are complete
            Timed.CallDelayed(GetFeedbackDuration(), callback, cullingObject);
        }

        public void Complete()
        {
            sounds?.ForEach(s => s.Complete());
            shakes?.ForEach(sh => sh.Complete());
            squash?.ForEach(sq => sq.Complete());
            colorGradients?.ForEach(cg => cg.Complete());
            cameraZooms?.ForEach(cz => cz.Complete());
            movements?.ForEach(e => e.Complete());
            effects?.ForEach(e => e.Complete());
        }

        public void Stop()
        {
            sounds?.ForEach(s => s.Stop());
            shakes?.ForEach(sh => sh.Stop());
            squash?.ForEach(sq => sq.Stop());
            colorGradients?.ForEach(cg => cg.Stop());
            cameraZooms?.ForEach(cz => cz.Stop());
            movements?.ForEach(e => e.Stop());
            effects?.ForEach(e => e.Stop());
        }
    }
}