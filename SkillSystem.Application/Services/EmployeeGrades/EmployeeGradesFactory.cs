using SkillSystem.Core.Entities;
using SkillSystem.Core.Enums;

namespace SkillSystem.Application.Services.EmployeeGrades;

public static class EmployeeGradesFactory
{
    public static EmployeeGrade InProgress(string employeeId, int gradeId)
    {
        return Create(employeeId, gradeId, GradeStatus.InProgress);
    }

    public static EmployeeGrade Achieved(string employeeId, int gradeId)
    {
        return Create(employeeId, gradeId, GradeStatus.Achieved);
    }

    public static EmployeeGrade Approved(string employeeId, int gradeId)
    {
        return Create(employeeId, gradeId, GradeStatus.Approved);
    }

    public static EmployeeGrade Create(string employeeId, int gradeId, GradeStatus gradeStatus)
    {
        return new EmployeeGrade { EmployeeId = employeeId, GradeId = gradeId, Status = gradeStatus };
    }
}