global using AutoMapper;
global using BuildingBlocks.Core.CQRS.Commands;
global using BuildingBlocks.Core.CQRS.Queries;
global using BuildingBlocks.Core.Exceptions.Base;
global using FluentValidation;
global using MediatR;
global using Ticket.Domain.Entities;
global using Ticket.Infrastructure.Context;
global using BuildingBlocks.Core.Validation;

namespace Ticket.Application;