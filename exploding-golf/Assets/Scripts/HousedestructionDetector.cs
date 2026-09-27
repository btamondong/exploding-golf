using UnityEngine;

public class HousedestructionDetector : MonoBehaviour
{
    public GameManager gameManager;

    private int totalPieces;
    private int threshold;

    void Start()
    {
        // Count all house pieces at the start
        totalPieces = GameObject.FindGameObjectsWithTag("HousePiece").Length;

        // Calculate 20% remaining threshold
        threshold = Mathf.FloorToInt(totalPieces * 0.2f);
    }

    void Update()
    {
        // Count remaining pieces
        int remaining = GameObject.FindGameObjectsWithTag("HousePiece").Length;

        // If 80% destroyed (remaining <= 20%), end the level
        if (remaining <= threshold)
        {
            gameManager.EndLevel();
        }
    }
}