using Jan.Core;
using UnityEngine;
using System.Collections.Generic;

namespace Giant.Feel
{
    public class FeedbackManager : Singleton<FeedbackManager>
    {
        [Tooltip("Default feedback asset used by the manager (optional).")]
        [SerializeField] GFeedback feedback;

        private readonly List<GFeedback> feedbacks = new List<GFeedback>();

        void Start()
        {
            feedbacks.Add(feedback);
        }

        public Feedback PlayFeedback(string feedbackName, Transform transform = null, Vector3 position = default, Quaternion rotation = default)
        {
            Feedback feedback = null;
            foreach (var item in feedbacks)
            {
                var fb = item.Play(feedbackName, transform, position, rotation);
                if (fb != null) feedback = fb;
            }

            if(feedback == null) Debug.LogWarning($"Feedback '{feedbackName}' not found.");
            return feedback;
        }

        public T GetFeedback<T>(string feedbackName, int index) where T : FeedbackBase
        {
            foreach (var item in feedbacks)
            {
                if (item.GetFB(feedbackName) is Feedback fb)
                {
                    return fb.GetFeedback<T>(index);
                }
            }

            Debug.LogWarning($"Feedback '{feedbackName}' not found.");
            return null;
        }

        public float GetFeedbackDuration(string feedbackName)
        {
            foreach (var item in feedbacks)
            {
                if (item.GetFB(feedbackName) is Feedback feedback)
                {
                    return feedback.GetFeedbackDuration();
                }
            }

            Debug.LogWarning($"Feedback '{feedbackName}' not found.");
            return 0f;
        }

        public void AddFeedback(GFeedback feedback)
        {
            feedbacks.Add(feedback);
        }

        public void RemoveFeedback(GFeedback feedback)
        {
            feedbacks.Remove(feedback);
        }
    }
}