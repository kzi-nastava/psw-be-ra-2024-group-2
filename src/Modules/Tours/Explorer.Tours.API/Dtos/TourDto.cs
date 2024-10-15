﻿namespace Explorer.Tours.API.Dtos;

public class TourDto
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public List<int> Equipment { get; set; } = new List<int>();
}
