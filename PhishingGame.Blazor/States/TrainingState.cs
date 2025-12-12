using System;
using System.Collections.Generic;
using PhishingGame.Core;

namespace PhishingGame.Blazor.States
{
    public class TrainingState
    {

        public Dictionary<string, List<Team>> TeamsByTraining { get; set; }
            = new Dictionary<string, List<Team>>(StringComparer.OrdinalIgnoreCase);

        public List<Team> GetOrCreateTeamsForTraining(string trainingName)
        {
            if (!TeamsByTraining.TryGetValue(trainingName, out var list))
            {
                list = new List<Team>();
                TeamsByTraining[trainingName] = list;
            }
            return list;
        }


        public List<Team> GetOrCreateTeams(string trainingName)
            => GetOrCreateTeamsForTraining(trainingName ?? string.Empty);
    
     public void SeedExampleTeams(string trainingName)
        {
            var teams = GetOrCreateTeams(trainingName ?? string.Empty);
            if (teams.Count > 0) return;

            teams.Add(new Team { Name = "Team Alpha", Score = 85 });
            teams.Add(new Team { Name = "Team Beta", Score = 90 });
            teams.Add(new Team { Name = "Team Gamma", Score = 75 });
        }
    }
}