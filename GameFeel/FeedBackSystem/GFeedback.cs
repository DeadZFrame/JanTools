using System.Collections.Generic;
using Jan.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jan.Feel
{
    public class GFeedback : SerializedMonoBehaviour
    {
        [SerializeField]
        [Tooltip("If true, this GameObject contains multiple named Feedbacks; otherwise a single Feedback is used.")]
        private bool multiple;

        [Space, ShowIf(nameof(multiple)), SerializeField]
        [Tooltip("List of Feedback entries when 'multiple' is enabled.")]
        private List<Feedback> feedbacks;

        [BoxGroup("Feedback", false), HideIf(nameof(multiple)), SerializeField]
        [Tooltip("Single Feedback used when 'multiple' is disabled.")]
        private Feedback feedback;

        [SerializeField]
        [Tooltip("Automatically play the default feedback on Start/Enable if true.")]
        private bool playOnStart;

        private void OnEnable()
        {
            if (multiple)
            {
                foreach (var fb in feedbacks)
                {
                    fb.Initialize(transform);
                    if (fb.listenEvent) EventManager.Register(fb.name, fb.Play);
                }
            }
            else
            {
                feedback?.Initialize(transform);
                if (feedback.listenEvent) EventManager.Register(feedback.name, feedback.Play);
            }

            //FeedbackManager.Instance.AddFeedback(this);

            if (playOnStart) feedback?.Play();
        }

        void OnDisable()
        {
            if (multiple)
            {
                foreach (var fb in feedbacks)
                {
                    if (fb.listenEvent) EventManager.UnRegister(fb.name, fb.Play);
                }
            }
            else
            {
                if (feedback.listenEvent) EventManager.UnRegister(feedback.name, feedback.Play);
            }

            //FeedbackManager.Instance.RemoveFeedback(this);
        }

        public Feedback Play(string feedbackName = "", Transform fbTransform = null, Vector3 fbPos = default, Quaternion rotation = default)
        {
            if (multiple)
            {
                if (string.IsNullOrEmpty(feedbackName))
                {
                    foreach (var fb in feedbacks)
                    {
                        if (fb.feedbackTransform == null) fb.feedbackTransform = transform;
                        fb.Play(fbTransform, fbPos, rotation);
                    }

                    return feedbacks[0]; // Return the first feedback as a reference
                }
                else
                {
                    var fb = GetFB(feedbackName);
                    if (fb == null) return null;
                    if (fb.feedbackTransform == null)
                        fb.feedbackTransform = transform;

                    fb.Play(fbTransform, fbPos, rotation);
                    return fb;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(feedbackName) || feedback.name == feedbackName)
                {
                    if (feedback.feedbackTransform == null) feedback.feedbackTransform = transform;
                    feedback.Play(fbTransform, fbPos, rotation);
                    return feedback;
                }
                else
                {
                    var fb = GetFB(feedbackName);
                    if (fb == null) return null;
                    if (fb.feedbackTransform == null) fb.feedbackTransform = transform;

                    fb.Play(fbTransform, fbPos, rotation);
                    return fb;
                }
            }
        }

        public Feedback GetFB(string feedbackName)
        {
            return feedbacks.Find(fb => fb.name == feedbackName);
        }

        public T GetFeedback<T>(int index, string name = "") where T : FeedbackBase
        {
            if (multiple)
            {
                foreach (var fb in feedbacks)
                {
                    if (string.IsNullOrEmpty(name) || fb.name.Equals(name))
                    {
                        return fb.GetFeedback<T>(index);
                    }
                }
            }
            else return feedback.GetFeedback<T>(index);

            return null;
        }

        public FeedbackBase[] GetFeedbacks()
        {
            if (multiple)
            {
                var feedbacks = new List<FeedbackBase>();
                foreach (var fb in this.feedbacks)
                {
                    feedbacks.AddRange(fb.GetFeedbacks());
                }
            }
            else return feedback.GetFeedbacks();

            return null;
        }

        public float GetFeedbackDuration()
        {
            float duration = 0;
            if (multiple)
            {
                foreach (var fb in feedbacks)
                {
                    float fbDuration = fb.GetFeedbackDuration();
                    if (fbDuration > duration)
                        duration = fbDuration;
                }
                return duration;
            }
            else return feedback.GetFeedbackDuration();
        }
        
        public void Complete()
        {
            if (multiple)
            {
                foreach (var fb in feedbacks)
                {
                    fb.Complete();
                }
            }
            else
            {
                feedback?.Complete();
            }
        }

        public void Stop()
        {
            if (multiple)
            {
                foreach (var fb in feedbacks)
                {
                    fb.Stop();
                }
            }
            else
            {
                feedback?.Stop();
            }
        }
    }
}