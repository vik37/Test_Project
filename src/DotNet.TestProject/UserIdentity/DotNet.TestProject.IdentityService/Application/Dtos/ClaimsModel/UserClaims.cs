﻿namespace DotNet.TestProject.IdentityService.Application.Dtos.ClaimsModel;

#pragma warning disable
public class UserClaims
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}