using UnityEngine;

public class MeatButton : MonoBehaviour
{
    public Meat meat;
    public Dog dog;

    public void OnMeatPressed()
    {
        meat.ShowMeat();
        dog.Distract();
    }
}