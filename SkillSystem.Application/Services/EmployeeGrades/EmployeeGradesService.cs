using Mapster;
using SkillSystem.Application.Common.Exceptions;
using SkillSystem.Application.Common.Extensions;
using SkillSystem.Application.Common.Services;
using SkillSystem.Application.Repositories.Grades;
using SkillSystem.Application.Services.EmployeeGrades.Models;
using SkillSystem.Application.Services.EmployeeSkills;
using SkillSystem.Core.Entities;
using SkillSystem.Core.Enums;

namespace SkillSystem.Application.Services.EmployeeGrades;

public class EmployeeGradesService : IEmployeeGradesService
{
    private readonly IEmployeeGradesRepository employeeGradesRepository;
    private readonly IGradesRepository gradesRepository;
    private readonly IEmployeeSkillsService employeeSkillsService;
    private readonly ICurrentUserProvider currentUserProvider;

    public EmployeeGradesService(
        IEmployeeGradesRepository employeeGradesRepository,
        IGradesRepository gradesRepository,
        IEmployeeSkillsService employeeSkillsService,
        ICurrentUserProvider currentUserProvider
    )
    {
        this.employeeGradesRepository = employeeGradesRepository;
        this.gradesRepository = gradesRepository;
        this.employeeSkillsService = employeeSkillsService;
        this.currentUserProvider = currentUserProvider;
    }

    public async Task AddEmployeeGradeAsync(string employeeId, int gradeId)
    {
        ThrowIfCurrentUserHasNotAccessTo(employeeId);

        var gradeToAdd = await gradesRepository.GetGradeByIdAsync(gradeId);
        var employeeGrade = await employeeGradesRepository.FindEmployeeGradeAsync(employeeId, gradeToAdd.Id)
                            ?? EmployeeGradesFactory.Achieved(employeeId, gradeId);

        var unachievedGrades = await GetUnachievedGradesAsync(employeeId, gradeToAdd);
        var skillsToAddIds = unachievedGrades.SelectMany(grade => grade.Skills.Select(skill => skill.Id));
        var unachievedGradesToAdd =
            unachievedGrades.Select(grade => EmployeeGradesFactory.Achieved(employeeId, grade.Id));

        var gradesToAdd = new List<EmployeeGrade>();
        gradesToAdd.AddRange(unachievedGradesToAdd);
        gradesToAdd.Add(employeeGrade);

        await employeeSkillsService.AddEmployeeSkillsAsync(employeeId, skillsToAddIds);
        await employeeGradesRepository.AddGradesAsync(gradesToAdd.ToArray());
    }

    public Task<ICollection<EmployeeGradeResponse>> FindEmployeeGradesAsync(string employeeId, int? roleId = null)
    {
        var employeeGrades = employeeGradesRepository.FindEmployeeGrades(employeeId, roleId);
        return Task.FromResult(employeeGrades.Adapt<ICollection<EmployeeGradeResponse>>());
    }

    public async Task<EmployeeGradeResponse> GetRoleLastGradeAsync(string employeeId, int roleId)
    {
        var lastEmployeeGrade = await employeeGradesRepository.GetLastRoleGradeAsync(employeeId, roleId);
        return lastEmployeeGrade.Adapt<EmployeeGradeResponse>();
    }

    public async Task ApproveEmployeeGradeAsync(string employeeId, int gradeId)
    {
        ThrowIfCurrentUserHasNotAccessTo(employeeId);

        var employeeGrade = await employeeGradesRepository.GetEmployeeGradeAsync(employeeId, gradeId);

        if (employeeGrade.Status == GradeStatus.InProgress)
            throw new ValidationException(
                $"Employee grade must have status {GradeStatus.Achieved.ToString()} to approve it"
            );

        var grades = await gradesRepository.GetGradesUntilAsync(employeeGrade.GradeId);
        var gradesBeforeIds = grades
            .SkipLast(1)
            .Select(nextGrade => nextGrade.Id);
        var employeeGradesBefore = await employeeGradesRepository.FindEmployeeGradesAsync(employeeId, gradesBeforeIds);
        var gradesToApprove = employeeGradesBefore
            .Concat(new[] { employeeGrade })
            .ToArray();
        foreach (var gradeToApprove in gradesToApprove)
            gradeToApprove.Status = GradeStatus.Approved;

        var skillsToApprove = grades
            .SelectMany(grade => grade.Skills.Select(skill => skill.Id));

        await employeeSkillsService.SetApprovedToSkillsAsync(employeeId, true, skillsToApprove);
        await employeeGradesRepository.UpdateGradesAsync(gradesToApprove);
    }

    private async Task<ICollection<Grade>> GetUnachievedGradesAsync(string employeeId, Grade untilGrade)
    {
        var lastEmployeeGrade = await employeeGradesRepository.FindLastRoleGradeAsync(employeeId, untilGrade.RoleId);
        var gradesBefore = await gradesRepository.GetGradesUntilAsync(untilGrade.Id);

        return lastEmployeeGrade is not null
            ? gradesBefore
                .SkipWhile(nextGrade => nextGrade.Id != lastEmployeeGrade.GradeId)
                .Skip(1)
                .ToArray()
            : gradesBefore;
    }

    private void ThrowIfCurrentUserHasNotAccessTo(string employeeId)
    {
        var currentUser = currentUserProvider.User?.GetUserId();
        if (currentUser != employeeId)
            throw new ForbiddenException($"Access denied for user with id {currentUser}");
    }
}