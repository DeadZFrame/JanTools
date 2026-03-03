using Jan.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

namespace Giant.Feel
{
    public class VFXAgent : MonoBehaviour
    {
        private VisualEffect[] _visualEffects;
        private ParticleSystem[] _particleSystems;
        private LineRenderer[] _lineRenderers;
        private TrailRenderer[] _trailRenderers;

    [Tooltip("If enabled, this agent will override spawned VFX default lifetime and delay settings.")]
    [SerializeField] private bool overrideSettings = false;
    [Tooltip("If overrideSettings is true, how long (seconds) before the spawned VFX is automatically stopped/despawned.")]
    [SerializeField, ShowIf(nameof(overrideSettings))] private float lifeTime = 0f;
    [Tooltip("If overrideSettings is true, delay (seconds) to wait before starting the spawned VFX.")]
    [SerializeField, ShowIf(nameof(overrideSettings))] private float delay = 0f;

        void OnEnable()
        {
            Setup();

            if (overrideSettings)
            {
                Timed.CallDelayed(delay, Play);
                Timed.CallDelayed(lifeTime, Stop);
            }
        }

        private void Setup()
        {
            _particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>(true);
            _visualEffects = gameObject.GetComponentsInChildren<VisualEffect>(true);
            _lineRenderers = gameObject.GetComponentsInChildren<LineRenderer>(true);
            _trailRenderers = gameObject.GetComponentsInChildren<TrailRenderer>(true); 
        }

        public void Play()
        {
            try
            {
                foreach (var v in _visualEffects) v.Play();
                foreach (var p in _particleSystems) p.Play();
                foreach (var l in _lineRenderers) l.gameObject.SetActive(true);
                foreach (var t in _trailRenderers) t.gameObject.SetActive(true);
            }
            catch
            {
                Setup();
                Play();
            }
        }

        public void Stop()
        {
            try
            {
                foreach (var v in _visualEffects) v.Stop();
                foreach (var p in _particleSystems) p.Stop();
                foreach (var l in _lineRenderers) l.gameObject.SetActive(false);
                foreach (var t in _trailRenderers) t.gameObject.SetActive(false);
            }
            catch
            {
                Setup();
                Stop();
            }
        }

        public void SetLifeTime(float lifeTime)
        {
            if(_particleSystems != null)
            {
                foreach (var p in _particleSystems)
                {
                    var main = p.main;
                    main.startLifetime = lifeTime;
                }
            }
            if (_visualEffects != null)
            {
                foreach (var v in _visualEffects) v.SetFloat("Lifetime", lifeTime);
            }
        }
        
        public void SetProgress(float progress)
        {
            if (_visualEffects != null)
            {
                foreach (var v in _visualEffects) v.SetFloat("Progress", progress);
            }
        }

        public void SetColor(Color color)
        {
            try 
            {
                if (_visualEffects != null)
                {
                    foreach (var v in _visualEffects)
                    {
                        if (v != null)
                        {
                            v.SetVector4("Color", color);
                        }
                    }
                }
                
                if (_particleSystems != null)
                {
                    foreach (var p in _particleSystems)
                    {
                        if (p != null)
                        {
                            var main = p.main;
                            main.startColor = color;

                            // Gradient renkleri de güncelle
                            var colorOverLifetime = p.colorOverLifetime;
                            if (colorOverLifetime.enabled)
                            {
                                var gradient = new ParticleSystem.MinMaxGradient(color);
                                colorOverLifetime.color = gradient;
                            }
                        }
                    }
                }

                if (_lineRenderers != null)
                {
                    foreach (var l in _lineRenderers)
                    {
                        if (l != null)
                        {
                            l.startColor = color;
                            l.endColor = color;
                        }
                    }
                }

                if (_trailRenderers != null)
                {
                    foreach (var t in _trailRenderers)
                    {
                        if (t != null)
                        {
                            t.startColor = color;
                            t.endColor = color;
                        }
                    }
                }
            }
            catch
            {
                Setup();
                SetColor(color);
            }
        }

        public float GetLongestLifeTime()
        {
            float longestLifeTime = 0f;

            if (_particleSystems != null)
            {
                foreach (var p in _particleSystems)
                {
                    if (p.main.loop) break;
                    float lifeTime = p.main.startLifetime.constant;
                    if (lifeTime > longestLifeTime)
                    {
                        longestLifeTime = lifeTime;
                    }
                }
            }

            if (_visualEffects != null)
            {
                foreach (var v in _visualEffects)
                {
                    if(v.GetBool("Loop")) break;
                    float lifeTime = v.GetFloat("Lifetime");
                    if (lifeTime > longestLifeTime)
                    {
                        longestLifeTime = lifeTime;
                    }
                }
            }

            return longestLifeTime;
        }
    }
}