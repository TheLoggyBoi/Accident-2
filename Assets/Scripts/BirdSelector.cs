using UnityEngine;
using UnityEngine.UI;

// Attach this to a UI panel that both players see.
// Each player's buttons call SelectBird(index) — this script
// figures out which bird GameObjects belong to THIS player and
// activates the right one, leaving the other player's birds alone.
public class BirdSelector : MonoBehaviour
{
    [System.Serializable]
    public struct BirdOption
    {
        public string name;
        public GameObject player1Bird;   // the bird GameObject for player 1
        public GameObject player2Bird;   // the bird GameObject for player 2
    }

    [Header("Bird Options (match order to your buttons)")]
    public BirdOption[] birds;

    [Header("Selection Buttons (same order as birds array)")]
    public Button[] buttons;

    void Start()
    {
        // Wire up buttons
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null)
                buttons[i].onClick.AddListener(() => SelectBird(index));
        }
    }

    public void SelectBird(int index)
    {
        if (index < 0 || index >= birds.Length) return;

        int myPlayer = GetMyPlayerNumber();
        if (myPlayer == 0) return; // not yet assigned

        for (int i = 0; i < birds.Length; i++)
        {
            bool isSelected = (i == index);

            if (myPlayer == 1 && birds[i].player1Bird != null)
                birds[i].player1Bird.SetActive(isSelected);
            else if (myPlayer == 2 && birds[i].player2Bird != null)
                birds[i].player2Bird.SetActive(isSelected);
        }

        Debug.Log($"Player {myPlayer} selected bird: {birds[index].name}");
    }

    int GetMyPlayerNumber()
    {
        if (TurnManager.Instance != null)
            return TurnManager.Instance.GetMyPlayerNumber();
        return 0;
    }
}
