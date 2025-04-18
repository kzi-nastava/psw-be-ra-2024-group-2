﻿using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.Core.Domain;

public class Tour  : Entity
{
    public long UserId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public TourDifficulty Difficulty { get; private set; }
    public TourTag Tag { get; private set; }                   
    public TourStatus Status { get; private set; }
    public double Price { get; private set; }
    public List<Equipment> Equipment { get; private set; } = new List<Equipment>();
    public List<Checkpoint> Checkpoints { get; private set; } = new List<Checkpoint>();
    public List<TourDurationByTransport>? TourDurationByTransports { get; set; } = new List<TourDurationByTransport>();
    public List<TourIssueReport>? TourIssueReports { get; private set; } = new List<TourIssueReport>();
    public DateTime? PublishedAt { get; private set; }
    public DateTime? ArchivedAt { get; private set; }
    public Tour(long userId, string name, string description, TourDifficulty difficulty, TourTag tag, TourStatus status, double price)
    {
        UserId = userId;
        Name = name;
        Description = description;
        Difficulty = difficulty;
        Tag = tag;
        Status = status;
        Price = price;
        Validate();
    }

    private void Validate()
    {
        if (UserId == 0) throw new ArgumentException("Invalid UserId");
        if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid TourName");
        if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid Description");
        if (Difficulty != TourDifficulty.Easy && Difficulty !=TourDifficulty.Moderate && Difficulty != TourDifficulty.Hard) throw new ArgumentException("Invalid Difficulty");
        if(Status != TourStatus.Draft && Status != TourStatus.Archived && Status != TourStatus.Published) throw new ArgumentException("Invalid Status");
        if(Tag != TourTag.Adventure && Tag != TourTag.Relaxation && Tag != TourTag.Historical && Tag != TourTag.Cultural && Tag != TourTag.Nature) throw new ArgumentException("Invalid Tag");
    }

    public void UpdateTransports(List<TourDurationByTransport> transports)
    {

        TourDurationByTransports.Clear();
        TourDurationByTransports.AddRange(transports);
    }

    public void UpdateCheckpoints(List<Checkpoint> checkpoints)
    {
        Checkpoints.Clear();
        Checkpoints.AddRange(checkpoints);
    }
   
    public void UpdateTour(TourStatus status, double price, List<Equipment> _equipment)
    {
        Status = status;
        Price = price;
        Equipment = _equipment;

        if (status.ToString() == "Published")
        {
            PublishedAt = DateTime.UtcNow;
            ArchivedAt = null;
        }
        else if (status.ToString() == "Archived")
        {
            PublishedAt = null;
            ArchivedAt = DateTime.UtcNow;
        }
    }


}