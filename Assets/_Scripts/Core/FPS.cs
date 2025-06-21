using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fpsText;
    [SerializeField] private float _updateRate = 4f;

    private float _timer;

    private void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            float fps = 1f / Time.unscaledDeltaTime;
            _fpsText.text = Mathf.Round(fps) + " FPS";
            _timer = Time.unscaledTime + 1f / _updateRate;
        }
    }
}
