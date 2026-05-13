using UnityEngine;

public class whitebird : MonoBehaviour
{
    public Rigidbody rb;
    public float downwardPushforce = 10f;
    public TicTacToeBoard TicTacToeBoard;
    public TicTacToeSquare TicTacToeSquare;

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("white bird worked");
            applydownwardPush();
        }
    }

    public void applydownwardPush()
    {
        if (rb != null)
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(Vector3.down * downwardPushforce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        TicTacToeSquare.ClearSquare();
    }
}
