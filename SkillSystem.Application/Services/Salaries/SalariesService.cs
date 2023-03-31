using Mapster;
using SkillSystem.Application.Repositories.Salaries;
using SkillSystem.Application.Services.Salaries.Models;
using SkillSystem.Core.Entities;

namespace SkillSystem.Application.Services.Salaries;

public class SalariesService : ISalariesService
{
    private readonly ISalariesRepository salariesRepository;

    public SalariesService(ISalariesRepository salariesRepository)
    {
        this.salariesRepository = salariesRepository;
    }

    public async Task<int> SaveSalaryAsync(SalaryRequest request)
    {
        var newSalary = request.Adapt<Salary>();
        if (newSalary.StartDate < DateTime.Now || (newSalary.StartDate.Month == DateTime.Now.Month &&
            newSalary.StartDate.Year == DateTime.Now.Year))
            return -1;
        var lastSalary = await salariesRepository.FindSalaryByMonthAsync(newSalary.EmployeeId,
            newSalary.StartDate);
        if (lastSalary != null && lastSalary.StartDate.Month == newSalary.StartDate.Month)
        {
            lastSalary.Wage = newSalary.Wage;
            lastSalary.Rate = newSalary.Rate;
            lastSalary.Bonus = newSalary.Bonus;
            lastSalary.StartDate = newSalary.StartDate;
            return await salariesRepository.UpdateSalaryAsync(lastSalary);
        } else
            return await salariesRepository.CreateSalaryAsync(newSalary);
    }

    public async Task<SalaryResponse> GetSalaryByIdAsync(int salaryId)
    {
        var salary = await salariesRepository.GetSalaryByIdAsync(salaryId);
        return salary.Adapt<SalaryResponse>();
    }

    public async Task<SalaryResponse> GetSalaryByMonthAsync(Guid employeeId, DateTime month)
    {
        var salary = await salariesRepository.GetSalaryByMonthAsync(employeeId, month);
        return salary.Adapt<SalaryResponse>();
    }

    public async Task<SalaryResponse> GetCurrentSalaryAsync(Guid employeeId)
    {
        var salary = await salariesRepository.GetCurrentSalaryAsync(employeeId);
        return salary.Adapt<SalaryResponse>();
    }

    public async Task<ICollection<SalaryResponse>> GetSalariesAsync(Guid employeeId, DateTime? from, DateTime? to)
    {
        var salaries = await salariesRepository.GetSalariesAsync(employeeId, from, to);
        var sortedSalaries = salaries.OrderBy(salary => salary.StartDate);
        return sortedSalaries.Adapt<ICollection<SalaryResponse>>();
    }

    public async Task CancelSalaryAssigmentAsync(int salaryId)
    {
        var salary = await salariesRepository.GetSalaryByIdAsync(salaryId);
        if (salary.StartDate < DateTime.Now || (salary.StartDate.Month == DateTime.Now.Month &&
            salary.StartDate.Year == DateTime.Now.Year))
            return;
        await salariesRepository.DeleteSalaryAsync(salary);
    }
}
