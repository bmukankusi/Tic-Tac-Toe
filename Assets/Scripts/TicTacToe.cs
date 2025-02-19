using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TicTacToe : MonoBehaviour
{
    public Button[] gridButtons;
    public Text displayText;
    public Button resetButton;
    public Button backToMenuButton;

    private string currentPlayer;
    private string[] board;
    private bool gameOver = false;

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

    public void ResetBoard()
    {
        gameOver = false;
        currentPlayer = "X"; // Player starts first
        displayText.text = "Next Player: X";
        resetButton.gameObject.SetActive(false);

        board = new string[9];

        for (int i = 0; i < gridButtons.Length; i++)
        {
            board[i] = "";
            gridButtons[i].GetComponentInChildren<Text>().text = "";
            gridButtons[i].GetComponent<Image>().color = Color.white;
            gridButtons[i].interactable = true;
        }
    }

    public void OnGridClick(int index)
    {
        if (gameOver || !string.IsNullOrEmpty(board[index])) return;

        // Player move
        board[index] = currentPlayer;
        gridButtons[index].GetComponentInChildren<Text>().text = currentPlayer;
        gridButtons[index].interactable = false;

        if (CheckWinner()) return;
        if (IsDraw()) return;

        // Switch to AI Player (O)
        currentPlayer = "O";
        displayText.text = "AI is Thinking...";

        // Delay AI move for better UX
        Invoke("MakeAIMove", 1.0f);
    }

    private void MakeAIMove()
    {
        if (gameOver) return;

        int bestMove = GetBestMove();
        board[bestMove] = "O";
        gridButtons[bestMove].GetComponentInChildren<Text>().text = "O";
        gridButtons[bestMove].interactable = false;

        if (CheckWinner()) return;
        if (IsDraw()) return;

        // Switch back to player (X)
        currentPlayer = "X";
        displayText.text = "Next Player: X";
    }

    private int GetBestMove()
    {
        int bestScore = int.MinValue;
        int move = -1;

        for (int i = 0; i < board.Length; i++)
        {
            if (string.IsNullOrEmpty(board[i]))
            {
                board[i] = "O";
                int score = Minimax(board, 0, false);
                board[i] = ""; // Undo move

                if (score > bestScore)
                {
                    bestScore = score;
                    move = i;
                }
            }
        }

        return move;
    }

    private int Minimax(string[] newBoard, int depth, bool isMaximizing)
    {
        string result = CheckWinState();
        if (result == "O") return 10 - depth; // AI wins
        if (result == "X") return depth - 10; // Player wins
        if (result == "Draw") return 0; // Tie

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < newBoard.Length; i++)
            {
                if (string.IsNullOrEmpty(newBoard[i]))
                {
                    newBoard[i] = "O";
                    int score = Minimax(newBoard, depth + 1, false);
                    newBoard[i] = ""; // Undo move
                    bestScore = Mathf.Max(score, bestScore);
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < newBoard.Length; i++)
            {
                if (string.IsNullOrEmpty(newBoard[i]))
                {
                    newBoard[i] = "X";
                    int score = Minimax(newBoard, depth + 1, true);
                    newBoard[i] = ""; // Undo move
                    bestScore = Mathf.Min(score, bestScore);
                }
            }
            return bestScore;
        }
    }

    private string CheckWinState()
    {
        int[,] winPatterns = {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Rows
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Columns
            {0, 4, 8}, {2, 4, 6}            // Diagonals
        };

        for (int i = 0; i < winPatterns.GetLength(0); i++)
        {
            int a = winPatterns[i, 0], b = winPatterns[i, 1], c = winPatterns[i, 2];
            if (!string.IsNullOrEmpty(board[a]) && board[a] == board[b] && board[a] == board[c])
            {
                return board[a]; // Returns "X" or "O"
            }
        }

        foreach (string cell in board)
        {
            if (string.IsNullOrEmpty(cell)) return null; // Game still ongoing
        }

        return "Draw";
    }

    private bool CheckWinner()
    {
        string winner = CheckWinState();
        if (winner == null) return false;

        if (winner == "X" || winner == "O")
        {
            int[,] winPatterns = {
                {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Rows
                {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Columns
                {0, 4, 8}, {2, 4, 6}            // Diagonals
            };

            for (int i = 0; i < winPatterns.GetLength(0); i++)
            {
                int a = winPatterns[i, 0], b = winPatterns[i, 1], c = winPatterns[i, 2];
                if (board[a] == winner && board[b] == winner && board[c] == winner)
                {
                    HighlightWinningCells(a, b, c);
                    break;
                }
            }

            displayText.text = $"{winner} Wins!";
            gameOver = true;
            resetButton.gameObject.SetActive(true);
            return true;
        }
        return false;
    }

    private bool IsDraw()
    {
        if (CheckWinState() == "Draw")
        {
            displayText.text = "It's a Draw!";
            gameOver = true;
            resetButton.gameObject.SetActive(true);
            return true;
        }
        return false;
    }

    private void HighlightWinningCells(int a, int b, int c)
    {
        gridButtons[a].GetComponent<Image>().color = Color.green;
        gridButtons[b].GetComponent<Image>().color = Color.green;
        gridButtons[c].GetComponent<Image>().color = Color.green;
    }

    //Back to menu scene
    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
}
