using UnityEngine;
public class audio_check : MonoBehaviour    
{
void Start() {
    // AICI E MAGIA: Accesăm instanța direct
    AudioManager.instance.PlaySound(AudioManager.instance.Bark1);
}

void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) {
        // AICI E MAGIA: Accesăm instanța direct
        AudioManager.instance.PlaySound(AudioManager.instance.Bark2);
    }
}   
}