using System;

class MancalaGame
{
    private int[] board;
    private int player1Store;
    private int player2Store;

    public MancalaGame()
    {
        board = new int[14]; // 12 pits + 2 stores
        player1Store = 6;    // Index for Player 1's store
        player2Store = 13;   // Index for Player 2's store

        // Initialize each pit with 4 stones
        for (int i = 0; i < 6; i++)
        {
            board[i] = 4;
            board[i + 7] = 4; // Pits for Player 2 are at index 7-12
        }
    }

    public void PlayGame()
    {
        bool gameOver = false;
        int currentPlayer = 1;

        while (!gameOver)
        {
            PrintBoard();

            Console.WriteLine($"Player {currentPlayer}'s turn.");
            Console.WriteLine("Choose a pit (1-6):");

            int pitChoice = int.Parse(Console.ReadLine()) - 1;

            if (IsValidMove(pitChoice, currentPlayer))
            {
                PlayTurn(pitChoice, currentPlayer);

                gameOver = CheckGameOver();

                // Switch players if the game isn't over
                if (!gameOver)
                {
                    currentPlayer = (currentPlayer == 1) ? 2 : 1;
                }
            }
            else
            {
                Console.WriteLine("Invalid move. Try again.");
            }
        }

        PrintBoard();
        DetermineWinner();
    }

    private bool IsValidMove(int pitChoice, int player)
    {
        // For Player 1, pits are 0-5; for Player 2, pits are 7-12
        if (player == 1 && pitChoice >= 0 && pitChoice <= 5 && board[pitChoice] > 0)
        {
            return true;
        }
        else if (player == 2 && pitChoice >= 7 && pitChoice <= 12 && board[pitChoice] > 0)
        {
            return true;
        }
        return false;
    }

    private void PlayTurn(int pitChoice, int currentPlayer)
    {
        int stones = board[pitChoice];
        board[pitChoice] = 0;

        int currentIndex = pitChoice;
        while (stones > 0)
        {
            currentIndex = (currentIndex + 1) % 14;

            // Skip the opponent's store
            if (currentPlayer == 1 && currentIndex == player2Store) continue;
            if (currentPlayer == 2 && currentIndex == player1Store) continue;

            board[currentIndex]++;
            stones--;
        }

        // If the last stone lands in the player's store, they get another turn
        if ((currentPlayer == 1 && currentIndex == player1Store) || (currentPlayer == 2 && currentIndex == player2Store))
        {
            Console.WriteLine($"Player {currentPlayer} gets another turn!");
            PlayGame();
        }

        // Capture rule: If the last stone lands in an empty pit on the player's side
        if (currentPlayer == 1 && currentIndex >= 0 && currentIndex <= 5 && board[currentIndex] == 1)
        {
            board[player1Store] += board[12 - currentIndex] + 1;
            board[12 - currentIndex] = 0;
            board[currentIndex] = 0;
        }
        else if (currentPlayer == 2 && currentIndex >= 7 && currentIndex <= 12 && board[currentIndex] == 1)
        {
            board[player2Store] += board[12 - currentIndex] + 1;
            board[12 - currentIndex] = 0;
            board[currentIndex] = 0;
        }
    }

    private void PrintBoard()
    {
        Console.WriteLine("\nBoard:");
        Console.WriteLine($"Player 2 Store: {board[player2Store]}");
        Console.WriteLine("Player 2 Pits: 12 11 10  9  8  7");
        Console.WriteLine($"            {board[12]}  {board[11]}  {board[10]}  {board[9]}  {board[8]}  {board[7]}");
        Console.WriteLine("-------------------------------");
        Console.WriteLine($"            {board[0]}  {board[1]}  {board[2]}  {board[3]}  {board[4]}  {board[5]}");
        Console.WriteLine("Player 1 Pits:  1  2  3  4  5  6");
        Console.WriteLine($"Player 1 Store: {board[player1Store]}\n");
    }

    private bool CheckGameOver()
    {
        int player1Total = 0, player2Total = 0;

        // Check if all pits on Player 1's side are empty
        for (int i = 0; i < 6; i++)
        {
            player1Total += board[i];
        }

        // Check if all pits on Player 2's side are empty
        for (int i = 7; i < 13; i++)
        {
            player2Total += board[i];
        }

        if (player1Total == 0 || player2Total == 0)
        {
            return true;
        }

        return false;
    }

    private void DetermineWinner()
    {
        // Add remaining stones to respective player's store
        for (int i = 0; i < 6; i++)
        {
            board[player1Store] += board[i];
            board[i] = 0;
        }

        for (int i = 7; i < 13; i++)
        {
            board[player2Store] += board[i];
            board[i] = 0;
        }

        Console.WriteLine("Game over!");

        if (board[player1Store] > board[player2Store])
        {
            Console.WriteLine("Player 1 wins!");
        }
        else if (board[player2Store] > board[player1Store])
        {
            Console.WriteLine("Player 2 wins!");
        }
        else
        {
            Console.WriteLine("It's a tie!");
        }
    }

    public static void Main(string[] args)
    {
        MancalaGame game = new MancalaGame();
        game.PlayGame();
    }
}
