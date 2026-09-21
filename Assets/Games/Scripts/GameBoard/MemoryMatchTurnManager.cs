
using System;

public class MemoryMatchTurnManager
{
    private readonly int _playerCount;
    private int _currentPlayerIndex;
    private int[] _playerScores;

    public int CurrentPlayerIndex => _currentPlayerIndex;

    public event Action<int, bool> OnTurnChanged;
    public event Action<int, int> OnScoreChanged;

    public MemoryMatchTurnManager(int playerCount)
    {
        _playerCount = playerCount;
        _playerScores = new int[_playerCount];

        _currentPlayerIndex = 0;
    }

    public void StartGame()
    {
        _currentPlayerIndex = 0;
        OnTurnChanged?.Invoke(_currentPlayerIndex, true);
    }

    public void OnMatchResult(bool isMatch)
    {
        if (isMatch)
        {
            // Correct pair:
            // Add score and current player continues.
            AddScore(_currentPlayerIndex);
        }
        else
        {
            // Wrong pair:
            // Switch to next player.
            SwitchToNextPlayer();
        }
    }

    private void AddScore(int playerIndex)
    {
        _playerScores[playerIndex]++;

        OnScoreChanged?.Invoke(
            playerIndex,
            _playerScores[playerIndex]
        );
    }

    private void SwitchToNextPlayer()
    {
        OnTurnChanged?.Invoke(_currentPlayerIndex, false);

        _currentPlayerIndex =
            (_currentPlayerIndex + 1) % _playerCount;

        OnTurnChanged?.Invoke(_currentPlayerIndex, true);
    }

    public int GetScore(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= _playerScores.Length)
            return 0;

        return _playerScores[playerIndex];
    }
}
