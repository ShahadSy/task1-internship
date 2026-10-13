using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace internship.Controllers;

[Authorize]
public class RolesController(RoleManager<IdentityRole> roleManager) : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<IActionResult> Index()
    {
        var roles = await _roleManager.Roles
            .OrderBy(role => role.Name)
            .ToListAsync();

        return View(roles);
    }

    public async Task<IActionResult> Details(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var role = await _roleManager.FindByIdAsync(id);

        if (role is null)
        {
            return NotFound();
        }

        return View(role);
    }
}
//This is the read-only Roles controller.