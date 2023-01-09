using Microsoft.AspNetCore.Mvc;
using SkillSystem.Application.Services.Projects;
using SkillSystem.Application.Services.Projects.Models;

namespace SkillSystem.WebApi.Controllers;

[Route("api/employees/{employeeId}/project-roles")]
public class EmployeeRolesController : BaseController
{
    private readonly IProjectRolesService projectRolesService;

    public EmployeeRolesController(IProjectRolesService projectRolesService)
    {
        this.projectRolesService = projectRolesService;
    }

    [HttpPost]
    public async Task<IActionResult> AddProjectRoleToEmployeeAsync(string employeeId, [FromBody] int projectRoleId)
    {
        await projectRolesService.SetEmployeeToProjectRoleAsync(projectRoleId, employeeId);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<ProjectRoleResponse>>> GetEmployeeProjectRolesAsync(
        string employeeId,
        [FromQuery] int? projectId = null
    )
    {
        var projectRoles = projectId != null
            ? await projectRolesService.FindRolesInProjectAsync(employeeId, projectId.Value)
            : await projectRolesService.GetEmployeeProjectRolesAsync(employeeId);
        return Ok(projectRoles);
    }
}