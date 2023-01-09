using SkillSystem.Core.Entities;

namespace SkillSystem.Application.Repositories.Grades;

public interface IEmployeeGradesRepository
{
    Task AddGradesAsync(params EmployeeGrade[] grades);
    Task<EmployeeGrade?> FindEmployeeGradeAsync(string employeeId, int gradeId);
    Task<EmployeeGrade> GetEmployeeGradeAsync(string employeeId, int gradeId);
    Task<ICollection<EmployeeGrade>> FindEmployeeGrades(string employeeId, int? roleId = null);
    Task<EmployeeGrade?> FindLastRoleGradeAsync(string employeeId, int roleId);
    Task<EmployeeGrade> GetLastRoleGradeAsync(string employeeId, int roleId);
    Task<ICollection<EmployeeGrade>> FindEmployeeGradesAsync(string employeeId, IEnumerable<int> gradesIds);
    Task UpdateGradesAsync(params EmployeeGrade[] employeeGrades);
    Task DeleteGradesAsync(params EmployeeGrade[] employeeGrades);
}