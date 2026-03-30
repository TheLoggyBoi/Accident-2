using JetBrains.Annotations;
using UnityEngine;

public class whitebird : MonoBehaviour
{
    public Rigidbody rb;
    public float downwardPushforce = 10f;
    public TicTacToeBoard TicTacToeBoard;

    private void Start()
    {
        TicTacToeBoard = GameObject.Find("TicTacToeBoard").GetComponent<TicTacToeBoard>();
    }

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
            rb.AddForce(Vector3.down*downwardPushforce, ForceMode.Impulse);
        }
    }

}
