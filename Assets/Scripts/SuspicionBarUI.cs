using UnityEngine;
using UnityEngine.UI;

public class SuspicionBarUI : MonoBehaviour
{
    public Image fillBar;

    void Start()
    {
        SuspicionSystem.Instance.OnSuspicionChanged += UpdateBar;
    }

    void UpdateBar(float value)
    {
        float normalized = value / SuspicionSystem.Instance.maxSuspicion;
        fillBar.fillAmount = normalized;
    }
}