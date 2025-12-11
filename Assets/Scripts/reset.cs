using UnityEngine;

public class reset : MonoBehaviour
{
    public Transform bird;
    public Transform resetpoint;
    public bool onboardhit;

    void Resetbird()
    {
        if (onboardhit == true)
        {
            bird = resetpoint;
        }
    }
}
