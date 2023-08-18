global using Ad.Application.Consts;
global using Ad.Application.Exceptions;
global using Ad.Application.Interfaces;
global using Ad.Domain.Enums;
global using BuildingBlocks.Core.CQRS.Commands;
global using BuildingBlocks.Core.CQRS.Queries;
global using BuildingBlocks.Core.Utilities.ImageRelated;
global using FluentValidation;
global using MediatR;
global using Microsoft.EntityFrameworkCore;
global using BuildingBlocks.Core.Utilities.FluentValidation;

namespace Ad.Application;