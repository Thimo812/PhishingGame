using PhishingGame.Core.Models;

namespace PhishingGame.Core;

public delegate void SessionUpdated(Session session);
public class Session(ILinkedState state, Training training, Guid hostId = default)
{
    private const int _minPlayersPerTeam = 1;
    private const int _maxPlayersPerTeam = 5;
    private const int _preferredTeamAmount = 3;

    public SessionData SessionData { get; set; } = new() { Training = training };

    public event SessionUpdated SessionEnded;
    public event SessionUpdated SessionStarted;
    public event SessionUpdated StateUpdated;
    public event SessionUpdated PlayerJoined;

    public ILinkedState CurrentState { get; set; } = state;
    public Guid SessionId { get; set; } = Guid.NewGuid();
    public Guid HostId { get; set; } = hostId;
    public bool CanJoin { get; set; } = true;

    public void AddPlayer(Guid id, string name)
    {
        SessionData.Players.Add(new Player(id, name));
        PlayerJoined?.Invoke(this);
    }

    public void Initialize()
    {
        CurrentState.InitializeState(this);
    }

    public async Task StartAsync()
    {
        CanJoin = false;
        DispatchMails();
        CreateTeams();

        SessionStarted?.Invoke(this);
    }

    public async Task NextStateAsync()
    {
        CurrentState = CurrentState.NextState;
        CurrentState.InitializeState(this);

        StateUpdated?.Invoke(this);
    }

    public void Stop()
    {
        CurrentState = null;
        SessionEnded?.Invoke(this);
    }

    public bool TryGetPlayer(Guid playerId, out Player player)
    {
        player = SessionData.Players.FirstOrDefault(player => player.Id == playerId);
        return player != null;
    }

    public bool ContainsPlayer(Player player)
    {
        return ContainsPlayer(player.Id);
    }

    public bool ContainsPlayer(Guid playerId)
    {
        return SessionData.Players.Any(player => player.Id == playerId);
    }

    private void CreateTeams()
    {
        int playerCount = SessionData.Players.Count;
        int teamAmount = GetPreferredTeamAmount(playerCount);

        for (int i = 1; i <= teamAmount; i++)
        {
            SessionData.Teams.Add(new Team { Name = $"Team {i}" });
        }

        int currentIndex = 0;

        while (currentIndex < playerCount)
        {
            foreach (var team in SessionData.Teams)
            {
                if (currentIndex >= playerCount) break;

                team.Players.Add(SessionData.Players[currentIndex]);
                currentIndex++;
            }
        }
    }

    private int GetPreferredTeamAmount(int playerCount)
    {
        List<int> preferredSizes = [];

        for (int i = _minPlayersPerTeam; i <= _maxPlayersPerTeam; i++)
        {
            if (playerCount % i == 0 && playerCount / i > 1)
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
                if ((bestDifference == null || difference < bestDifference) && playerCount / size > 1)
                {
                    bestSize = size;
                }
            }

            if (bestSize != null) return bestSize.Value;
        }

        return _preferredTeamAmount;
    }

    private void DispatchMails()
    {
        var normalMails = new List<Email>();
        var phishingMails = new List<Email>();

        foreach (var mail in SessionData.Training.Emails)
        {
            if (mail.IsPhishing)
            {
                phishingMails.Add(mail);
            }
            else normalMails.Add(mail);
        }

        int currentIndex = 0;

        AddMails(normalMails, ref currentIndex);
        AddMails(phishingMails, ref currentIndex);
    }

    private void AddMails(List<Email> mailSource, ref int currentIndex)
    {
        foreach (var mail in mailSource)
        {
            var team = SessionData.Teams.ElementAtOrDefault(currentIndex++);
            if (team == null) return;
            if (!SessionData.Mails.TryGetValue(team, out var mails)) continue;

            mails.Add(mail);
        }
    }
}
