using UnityEngine;
using UnityEngine.UI;
//text mesh pro
using TMPro;

public class TicTacToe : MonoBehaviour
{
    public Button[] gridButtons;
    public Text displayText;
    private string currentPlayer = "X";
    private string[] board = new string[9];
    private bool gameOver = false;
    public Button resetButton;


    void Start()
    {
        ResetBoard();

        for (int i = 0; i < gridButtons.Length; i++)
        {
            int index = i;
            gridButtons[i].onClick.RemoveAllListeners();
            gridButtons[i].onClick.AddListener(() => OnGridClick(index));
        }

        resetButton.onClick.RemoveAllListeners();
        resetButton.onClick.AddListener(ResetBoard);
    }


    public void OnGridClick(int index)
    {
        if (gameOver || !string.IsNullOrEmpty(board[index])) return;

        board[index] = currentPlayer;
        gridButtons[index].GetComponentInChildren<Text>().text = currentPlayer;
        gridButtons[index].interactable = false; // Disable button after selection

        if (CheckWinner())
        {
            displayText.text = $"{currentPlayer} Wins!";
            gameOver = true;
        }
        else if (IsDraw())
        {
            displayText.text = "It's a Draw!";
            gameOver = true;
        }
        else
        {
            currentPlayer = (currentPlayer == "X") ? "O" : "X";
            displayText.text = $"Next Player: {currentPlayer}";
        }
    }

    private bool CheckWinner()
    {
        int[,] winPatterns = {
        {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Rows
        {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Columns
        {0, 4, 8}, {2, 4, 6}            // Diagonals
    };

        for (int i = 0; i < winPatterns.GetLength(0); i++)
        {
            int a = winPatterns[i, 0], b = winPatterns[i, 1], c = winPatterns[i, 2];
            if (board[a] == currentPlayer && board[b] == currentPlayer && board[c] == currentPlayer)
            {
                HighlightWinningCells(a, b, c);
                displayText.text = $"{currentPlayer} Wins!";
                gameOver = true;
                resetButton.gameObject.SetActive(true); // Show reset button
                return true;
            }
        }
        return false;
    }


    private bool IsDraw()
    {
        foreach (string cell in board)
        {
            if (string.IsNullOrEmpty(cell)) return false;
        }
        displayText.text = "It's a Draw!";
        gameOver = true;
        resetButton.gameObject.SetActive(true); // Show reset button
        return true;
    }


    private void HighlightWinningCells(int a, int b, int c)
    {
        Color winColor = new Color(0.5f, 1f, 0.5f); // Light Green
        gridButtons[a].GetComponent<Image>().color = winColor;
        gridButtons[b].GetComponent<Image>().color = winColor;
        gridButtons[c].GetComponent<Image>().color = winColor;
    }

    public void ResetBoard()
    {
        gameOver = false;
        currentPlayer = "X";
        displayText.text = "Next Player: X";
        resetButton.gameObject.SetActive(false); // Hide reset button

        for (int i = 0; i < gridButtons.Length; i++)
        {
            board[i] = "";
            gridButtons[i].GetComponentInChildren<Text>().text = "";
            gridButtons[i].GetComponent<Image>().color = Color.white;
            gridButtons[i].interactable = true;
        }
    }

}
