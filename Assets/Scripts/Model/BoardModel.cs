using UnityEngine;

public class BoardModel : MonoBehaviour
{
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;
    [SerializeField] private GameObject whiteSquarePrefab;
    [SerializeField] private GameObject blackSquarePrefab;

    private Board board;

    void Start()
    {
        board = new Board(width, height, whiteSquarePrefab, blackSquarePrefab);
    }
}
