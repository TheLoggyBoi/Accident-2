using UnityEngine;

public class blackboard : MonoBehaviour
{
    // Drag and drop your X/O container objects or assign them in script
    public GameObject[] symbols;
    public TicTacToeBoard TicTacToeBoard;


    void Start()
    {
        TicTacToeBoard = GameObject.Find("TicTacToeBoard").GetComponent<TicTacToeBoard>();
    }
    void OnCollisionEnter(Collision collision)
    {
        // Check if the character hits the board
        if (collision.gameObject.CompareTag("Bomb"))
        {
            TicTacToeBoard.ResetBoard();
            Debug.Log("Bomb Worked");
        }
    }

    void ClearBoard()
    {
        // Deactivate all symbols on the board
        foreach (GameObject symbol in symbols)
        {
            symbol.SetActive(true);
        }
        Debug.Log("Board Cleared!");
        // Reset your game logic here
    }
}
