﻿using Opsphere.Data.Models;

namespace Opsphere.Dtos.Project;

public class projectDtoWithdevs
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<ProjectDeveloper>? ProjectDevelopers { get; set; }
}