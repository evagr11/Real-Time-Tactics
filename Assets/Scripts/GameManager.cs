using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Board board;
    [SerializeField] int boardWidth;
    [SerializeField] int boardHeight;
    [SerializeField] GameObject boardSquarePrefab;


    void Start()
    {
        board = new Board(boardWidth, boardHeight, boardSquarePrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
