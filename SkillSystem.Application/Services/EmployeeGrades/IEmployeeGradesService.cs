using SkillSystem.Application.Services.EmployeeGrades.Models;

namespace SkillSystem.Application.Services.EmployeeGrades;

public interface IEmployeeGradesService
{
    Task AddEmployeeGradeAsync(string employeeId, int gradeId);
    Task<ICollection<EmployeeGradeResponse>> FindEmployeeGradesAsync(string employeeId, int? roleId = null);
    Task<EmployeeGradeResponse> GetRoleLastGradeAsync(string employeeId, int roleId);
    Task ApproveEmployeeGradeAsync(string employeeId, int gradeId);
}