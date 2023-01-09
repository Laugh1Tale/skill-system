using Microsoft.EntityFrameworkCore;
using SkillSystem.Application.Common.Exceptions;
using SkillSystem.Application.Repositories.Grades;
using SkillSystem.Core.Entities;

namespace SkillSystem.Infrastructure.Persistence.Repositories;

public class EmployeeGradesRepository : IEmployeeGradesRepository
{
    private readonly SkillSystemDbContext dbContext;

    public EmployeeGradesRepository(SkillSystemDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddGradesAsync(params EmployeeGrade[] grades)
    {
        foreach (var grade in grades)
        {
            var presentGrade = await FindEmployeeGradeAsync(grade.EmployeeId, grade.GradeId);
            if (presentGrade is null)
                await dbContext.EmployeeGrades.AddAsync(grade);
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task<EmployeeGrade?> FindEmployeeGradeAsync(string employeeId, int gradeId)
    {
        return await QueryEmployeeGrades(employeeId)
            .FirstOrDefaultAsync(employeeGrade => employeeGrade.GradeId == gradeId);
    }

    public async Task<EmployeeGrade> GetEmployeeGradeAsync(string employeeId, int gradeId)
    {
        return await FindEmployeeGradeAsync(employeeId, gradeId)
               ?? throw new EntityNotFoundException(nameof(EmployeeGrade), new { employeeId, gradeId });
    }

    public async Task<ICollection<EmployeeGrade>> FindEmployeeGrades(string employeeId, int? roleId = null)
    {
        return await QueryEmployeeGrades(employeeId, roleId).ToListAsync();
    }

    public async Task<EmployeeGrade?> FindLastRoleGradeAsync(string employeeId, int roleId)
    {
        var employeeGrades = await QueryEmployeeGrades(employeeId, roleId).ToListAsync();
        var employeeGradeIds = employeeGrades
            .Select(employeeGrade => employeeGrade.GradeId)
            .ToHashSet();
        var lastEmployeeGrade = employeeGrades.FirstOrDefault(
            employeeGrade => !employeeGrade.Grade.NextGradeId.HasValue
                             || !employeeGradeIds.Contains(employeeGrade.Grade.NextGradeId.Value)
        );
        return lastEmployeeGrade;
    }

    public async Task<EmployeeGrade> GetLastRoleGradeAsync(string employeeId, int roleId)
    {
        return await FindLastRoleGradeAsync(employeeId, roleId)
               ?? throw new EntityNotFoundException(nameof(EmployeeGrade), new { employeeId, roleId });
    }

    public async Task<ICollection<EmployeeGrade>> FindEmployeeGradesAsync(string employeeId, IEnumerable<int> gradesIds)
    {
        return await QueryEmployeeGrades(employeeId)
            .Where(employeeGrade => gradesIds.Contains(employeeGrade.GradeId))
            .ToListAsync();
    }

    public async Task UpdateGradesAsync(params EmployeeGrade[] employeeGrades)
    {
        dbContext.EmployeeGrades.UpdateRange(employeeGrades);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteGradesAsync(params EmployeeGrade[] employeeGrades)
    {
        dbContext.EmployeeGrades.RemoveRange(employeeGrades);
        await dbContext.SaveChangesAsync();
    }

    private IQueryable<EmployeeGrade> QueryEmployeeGrades(string employeeId, int? roleId = null)
    {
        var query = dbContext.EmployeeGrades
            .Include(employeeGrade => employeeGrade.Grade)
            .Where(employeeGrade => employeeGrade.EmployeeId == employeeId);

        if (roleId.HasValue)
            query = query.Where(employeeGrade => employeeGrade.Grade.RoleId == roleId);

        return query;
    }
}