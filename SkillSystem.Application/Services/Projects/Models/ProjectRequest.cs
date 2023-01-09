using System.ComponentModel.DataAnnotations;

namespace SkillSystem.Application.Services.Projects.Models;

public record ProjectRequest
{
    [Required]
    [MaxLength(30)]
    public string Name { get; init; }
}