﻿global using System.Linq.Expressions;
global using Authentication.Application.DTOs.ResponseDto;
global using Authentication.Domain.Enums;
global using Authentication.Application.DTOs.RequestDto;
global using FluentValidation;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Newtonsoft.Json;
global using System.Text.RegularExpressions;
global using Authentication.Application.StaticClassHelper;
global using System.Globalization;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;
global using Authentication.Domain.Models;
global using Microsoft.Extensions.Configuration;
global using Microsoft.IdentityModel.Tokens;
global using Authentication.Application.Contracts.ExternalServicesContract;
global using Authentication.Application.Contracts.RepositoryContracts;
global using Authentication.Application.Contracts.UtilityContracts;
global using Authentication.Application.StaticClassHelper.EmailTemplate;
global using MediatR;