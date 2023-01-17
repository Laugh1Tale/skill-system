﻿using Microsoft.EntityFrameworkCore;
using SkillSystem.Application.Common.Exceptions;
using SkillSystem.Application.Repositories.Grades;
using SkillSystem.Core.Entities;

namespace SkillSystem.Infrastructure.Persistence.Repositories;

public class GradesRepository : IGradesRepository
{
    private readonly SkillSystemDbContext dbContext;

    public GradesRepository(SkillSystemDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Grade?> FindGradeByIdAsync(int gradeId)
    {
        return await dbContext.Grades
            .Include(grade => grade.PrevGrade)
            .Include(grade => grade.NextGrade)
            .Include(grade => grade.Skills.OrderBy(skill => skill.Id))
            .ThenInclude(grade => grade.SubSkills.OrderBy(skill => skill.Id))
            .FirstOrDefaultAsync(grade => grade.Id == gradeId);
    }

    public async Task<Grade> GetGradeByIdAsync(int gradeId)
    {
        return await FindGradeByIdAsync(gradeId) ?? throw new EntityNotFoundException(nameof(Grade), gradeId);
    }

    public IQueryable<Grade> FindGrades(string? title = default)
    {
        var query = dbContext.Grades.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(grade => grade.Title.Contains(title));

        return query.OrderBy(grade => grade.Id);
    }

    public async Task<ICollection<Grade>> GetGradesUntilAsync(int gradeId)
    {
        var grade = await dbContext.Grades
            .Include(grade => grade.Skills)
            .Include(grade => grade.PrevGrade)
            .FirstOrDefaultAsync(grade => grade.Id == gradeId);

        grade = grade ?? throw new EntityNotFoundException(nameof(Grade), gradeId);

        return EnumeratePrevGrades(grade);
    }

    public async Task<IEnumerable<Skill>> GetGradeSkillsAsync(int gradeId)
    {
        var gradeSkills = await dbContext.Grades
            .AsNoTracking()
            .Include(grade => grade.Skills.OrderBy(skill => skill.Id))
            .ThenInclude(skill => skill.SubSkills.OrderBy(subSkill => subSkill.Id))
            .Where(grade => grade.Id == gradeId)
            .Select(grade => grade.Skills)
            .FirstOrDefaultAsync();

        if (gradeSkills is null)
            throw new EntityNotFoundException(nameof(Grade), gradeId);

        return gradeSkills;
    }

    public async Task<ICollection<Position>> GetGradePositionsAsync(int gradeId)
    {
        var gradePositions = await dbContext.Grades
            .AsNoTracking()
            .Include(grade => grade.Positions.OrderBy(position => position.Id))
            .Where(grade => grade.Id == gradeId)
            .Select(grade => grade.Positions)
            .FirstOrDefaultAsync();

        if (gradePositions is null)
            throw new EntityNotFoundException(nameof(Grade), gradeId);

        return gradePositions;
    }

    public async Task UpdateGradeAsync(Grade grade)
    {
        dbContext.Grades.Update(grade);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddGradeSkillAsync(int gradeId, Skill skill)
    {
        var grade = await GetGradeByIdAsync(gradeId);

        var gradeSkill = new GradeSkill
        {
            GradeId = grade.Id,
            SkillId = skill.Id
        };

        await dbContext.GradeSkills.AddAsync(gradeSkill);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteGradeSkillAsync(int gradeId, int skillId)
    {
        dbContext.GradeSkills.Remove(new GradeSkill { GradeId = gradeId, SkillId = skillId });
        await dbContext.SaveChangesAsync();
    }

    public async Task AddGradePositionAsync(int gradeId, Position position)
    {
        var grade = await GetGradeByIdAsync(gradeId);
        var currentPositionGrade = await FindCurrentPositionGrade(grade.RoleId, position.Id);

        if (currentPositionGrade is not null)
        {
            dbContext.PositionGrades.Remove(currentPositionGrade);
        }

        var newPositionGrade = new PositionGrade
        {
            GradeId = grade.Id,
            PositionId = position.Id
        };
        await dbContext.PositionGrades.AddAsync(newPositionGrade);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteGradePositionAsync(int gradeId, int positionId)
    {
        dbContext.PositionGrades.Remove(new PositionGrade { PositionId = positionId, GradeId = gradeId });
        await dbContext.SaveChangesAsync();
    }

    private static ICollection<Grade> EnumeratePrevGrades(Grade grade)
    {
        var grades = new List<Grade>();
        var currentGrade = grade;
        while (currentGrade is not null)
        {
            grades.Add(currentGrade);
            currentGrade = currentGrade.PrevGrade;
        }

        grades.Reverse();
        return grades;
    }

    private async Task<PositionGrade?> FindCurrentPositionGrade(int roleId, int positionId)
    {
        return await dbContext.PositionGrades
            .FirstOrDefaultAsync(
                positionGrade => positionGrade.Grade.RoleId == roleId && positionGrade.PositionId == positionId
            );
    }
}