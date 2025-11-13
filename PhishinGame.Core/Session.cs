using PhishingGame.Core.Models;

namespace PhishingGame.Core;

public delegate void SessionUpdated(Session session);
public class Session(ILinkedState state, Training training)
{
    private const int _minPlayersPerTeam = 1;
    private const int _maxPlayersPerTeam = 5;
    private const int _preferredTeamAmount = 3;

    private SessionData _sessionData = new() { Training = training };

    public event SessionUpdated SessionEnded;
    public event SessionUpdated SessionStarted;
    public event SessionUpdated StateUpdated;

    public ILinkedState CurrentState { get; set; } = state;
    public Guid SessionId { get; set; } = Guid.NewGuid();
    public Guid HostId { get; set; }
    public bool CanJoin { get; set; } = true;

    public void AddPlayer(Guid id, string name)
    {
        _sessionData.Players.Add(new Player(id, name));
    }

    public async Task StartAsync()
    {
        CanJoin = false;
        CreateTeams();
        CurrentState.InitializeState(_sessionData);

        SessionStarted?.Invoke(this);
    }

    public async Task ContinueAsync()
    {
        CurrentState = CurrentState.NextState;
        CurrentState.InitializeState(_sessionData);

        StateUpdated?.Invoke(this);
    }

    public async Task StopAsync()
    {
        CurrentState = null;
        SessionEnded?.Invoke(this);
    }

    public bool ContainsPlayer(Player player)
    {
        return ContainsPlayer(player.Id);
    }

    public bool ContainsPlayer(Guid playerId)
    {
        return _sessionData.Players.Any(player => player.Id == playerId);
    }

    private void CreateTeams()
    {
        int playerCount = _sessionData.Players.Count;
        int teamAmount = GetPreferredTeamAmount(playerCount);

        for (int i = 0; i < teamAmount; i++)
        {
            _sessionData.Teams.Add(new Team { Name = $"Team {i}" });
        }

        int currentIndex = 0;

        while (currentIndex < playerCount)
        {
            foreach (var team in _sessionData.Teams)
            {
                if (currentIndex >= playerCount) break;

                team.Players.Add(_sessionData.Players[currentIndex]);
                currentIndex++;
            }
        }
    }

    private int GetPreferredTeamAmount(int playerCount)
    {
        if (playerCount < 4)
        {
            return 1;
        }

        List<int> preferredSizes = [];

        for (int i = _minPlayersPerTeam; i <= _maxPlayersPerTeam; i++)
        {
            if (playerCount % i == 0)
            {
                preferredSizes.Add(i);
            }
        }

        if (preferredSizes.Count > 0)
        {
            int? bestSize = null;
            int? bestDifference = null;

            foreach (int size in preferredSizes)
            {
                int difference = Math.Abs(size - _preferredTeamAmount);
                if ((bestDifference != null || difference < bestDifference) && playerCount / size > 1)
                {
                    bestSize = size;
                }
            }

            if (bestSize != null) return bestSize.Value;
        }

        return _preferredTeamAmount;
    }
}
