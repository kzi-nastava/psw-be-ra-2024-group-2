﻿using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Stakeholders.Core.Domain;
public class User : Entity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
    public bool IsBlocked {  get; set; }

    public User(string username, string password, UserRole role, bool isActive)
    {
        Username = username;
        Password = password;
        Role = role;
        IsActive = isActive;
        IsBlocked = false;
        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Username)) throw new ArgumentException("Invalid Name");
        if (string.IsNullOrWhiteSpace(Password)) throw new ArgumentException("Invalid Surname");
    }

    public string GetPrimaryRoleName()
    {
        return Role.ToString().ToLower();
    }
}