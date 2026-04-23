using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    public GameObject panelToClose;

    public void ClosePanel()
    {
        panelToClose.SetActive(false);
    }
}