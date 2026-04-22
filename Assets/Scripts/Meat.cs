using UnityEngine;
using System.Collections;

public class Meat : MonoBehaviour
{
    public Vector3 hiddenPosition;
    public Vector3 visiblePosition;
    public float moveSpeed = 5f;

    private bool isActive;

    void Start()
    {
        transform.position = hiddenPosition;
        isActive = false;
    }

    public void ShowMeat()
    {
        StopAllCoroutines();
        StartCoroutine(MoveTo(visiblePosition));
        isActive = true;
    }

    public void HideMeat()
    {
        StopAllCoroutines();
        StartCoroutine(MoveTo(hiddenPosition));
        isActive = false;
    }

    IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                target,
                Time.deltaTime * moveSpeed
            );

            yield return null;
        }

        transform.position = target;
    }

    public bool IsActive()
    {
        return isActive;
    }
}