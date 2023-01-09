using SkillSystem.Application.Services.Grades.Models;
using SkillSystem.Core.Enums;

namespace SkillSystem.Application.Services.EmployeeGrades.Models;

public record EmployeeGradeResponse
{
    public GradeShortInfo Grade { get; init; }
    public GradeStatus GradeStatus { get; init; }
}