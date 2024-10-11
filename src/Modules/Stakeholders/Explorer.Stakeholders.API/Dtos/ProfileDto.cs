using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos;

public class ProfileDto
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Biography { get; set; }
    public string? Moto { get; set; }
    public ImageDto? Image { get; set; } 
}
