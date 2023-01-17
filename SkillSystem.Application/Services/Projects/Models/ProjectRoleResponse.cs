using SkillSystem.Application.Services.Roles.Models;

namespace SkillSystem.Application.Services.Projects.Models;

public record ProjectRoleResponse
{
    public int Id { get; init; }
    public ProjectShortInfo Project { get; init; }
    public RoleShortInfo Role { get; init; }
    public string? EmployeeId { get; init; }
}