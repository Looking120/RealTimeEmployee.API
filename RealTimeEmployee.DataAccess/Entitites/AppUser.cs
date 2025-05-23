﻿using Microsoft.AspNetCore.Identity;

namespace RealTimeEmployee.DataAccess.Entitites;

public class AppUser : IdentityUser<Guid>
{
    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public string? MiddleName { get; init; }

    public Guid? EmployeeId { get; set; }

    public Employee? Employee { get; set; }
}
