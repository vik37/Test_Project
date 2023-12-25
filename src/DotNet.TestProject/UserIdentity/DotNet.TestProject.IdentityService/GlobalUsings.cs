// System Using Namespaces
global using System.Reflection;
global using System.Text;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;

// Microsoft Using Namespaces
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Data.SqlClient;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.Extensions.Options;
global using Microsoft.OpenApi.Models;

// Another Nuget Using Namespaces
global using Asp.Versioning;
global using Newtonsoft.Json.Serialization;
global using Serilog;
global using MediatR;
global using AutoMapper;
global using FluentValidation;

// Custom Using Namespaces
global using DotNet.TestProject.IdentityService;
global using DotNet.TestProject.IdentityDomainModels.Models;
global using DotNet.TestProject.IdentityInfrastructures;
global using DotNet.TestProject.IdentityService.Application.Dtos.Enums;
global using DotNet.TestProject.IdentityService.Application.Command;
global using DotNet.TestProject.IdentityDomainModels.UserDomainCustomException;
global using DotNet.TestProject.IdentityService.Application.Behavior;
global using DotNet.TestProject.IdentityService.Application.Validators;
global using DotNet.TestProject.IdentityService.Infrastructure;
global using DotNet.TestProject.IdentityService.Infrastructure.OptionsModel;
global using DotNet.TestProject.IdentityService.Application.Dtos;
global using DotNet.TestProject.IdentityService.Infrastructure.AuthenticationHelper;
global using DotNet.TestProject.IdentityService.Application.Dtos.ClaimsModel;
global using DotNet.TestProject.IdentityService.Application.Queries.ViewModels;
global using DotNet.TestProject.IdentityService.Application.Queries;
global using DotNet.TestProject.IdentityService.Application.DomainEvents;