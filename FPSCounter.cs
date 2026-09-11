using Jan.Tasks;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private float PollingIncrement = 0.1f;
    [SerializeField] private TextMeshProUGUI fpsText;

    private void Start()
    {
        Timed.CallPeriodically(int.MaxValue, PollingIncrement, () =>
        {
            var fps = 1f / Time.unscaledDeltaTime;
            fpsText.SetText($"{fps:0.0} FPS");
        });
    }
}