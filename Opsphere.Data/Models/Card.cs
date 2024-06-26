﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Opsphere.Data.Models;

public enum Status
{
    Todo,
    InProgress,
    Done
}

public class Card
{
    [Key]
    public int Id { get; set; }
    [StringLength(50)]
    [Required]
    public string Title { get; set; } = string.Empty;
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [JsonConverter(typeof(StringEnumConverter))]
    public Status Status { get; set; } = Status.Todo;
    [Required]
    [ForeignKey(nameof(Project))]
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
    [ForeignKey(nameof(AssignedDeveloper))]
    public int? AssignedDeveloperId { get; set; }
    public ProjectDeveloper? AssignedDeveloper { get; set; }
    
    public ICollection<CardComment>? CardComments { get; set; }
    public Attachment? Attachment { get; set; }
}