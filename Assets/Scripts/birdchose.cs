using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class birdchose : MonoBehaviour
{
    public Dropdown OptionA;
    public Dropdown OptionB;
    public Dropdown OptionC;
    public Dropdown OptionD;

    public Transform terry1;
    public Transform terry2;
    public Transform Detonator1;
    public Transform Detonator2;
    public Transform TicTacToeBirds1;
    public Transform TicTacToeBirds2;
    public Transform darcy1;
    public Transform darcy2;
    public Transform BirdSlingshotLocatoin;


    void Update()
    {
        if (OptionA != null && OptionB != null && OptionC != null)
        {
            TicTacToeBirds1.position = BirdSlingshotLocatoin.position;
        }
    }
}
