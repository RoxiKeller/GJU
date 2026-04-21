using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void LateUpdate()
    {
        // Keeps the text straight even if the NPC parent rotates
        transform.rotation = Quaternion.identity; 
    }
}
