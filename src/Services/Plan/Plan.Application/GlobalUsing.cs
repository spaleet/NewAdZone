﻿global using AutoMapper;
global using BuildingBlocks.Core.CQRS.Commands;
global using BuildingBlocks.Core.CQRS.Queries;
global using BuildingBlocks.Core.Exceptions.Base;
global using BuildingBlocks.Core.Validation;
global using BuildingBlocks.Payment;
global using FluentValidation;
global using MediatR;
global using Plan.Domain.Entities;
global using Plan.Infrastructure.Context;

namespace Plan.Application;