﻿using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain;

public class Equipment : Entity
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public List<Tour> Tours { get; private set; } = new List<Tour>();

    public Equipment(string name, string? description)
    {
        if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid TourName.");
        Name = name;
        Description = description;
    }
}