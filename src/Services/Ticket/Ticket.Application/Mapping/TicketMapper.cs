using BuildingBlocks.Core.Utilities;
using Ticket.Application.Dtos;
using Ticket.Application.Features.Admin.GettingDetails;
using Ticket.Application.Features.User.GettingDetails;
using Ticket.Application.Features.User.PostingTicket;

namespace Ticket.Application.Mapping;

public class TicketMapper : Profile
{
    public TicketMapper()
    {
        CreateMap<PostTicket, Domain.Entities.Ticket>();

        CreateMap<Domain.Entities.Ticket, TicketDto>();

        CreateMap<TicketMessage, TicketMessageDto>()
            .ForMember(dest => dest.SentDate,
                           opt => opt.MapFrom(src => src.CreationDate.ToLongShamsi()));

        CreateMap<Domain.Entities.Ticket, GetTicketDetailsUserResponse>()
            .ForMember(dest => dest.Messages,
                           opt => opt.Ignore())
            .ForMember(dest => dest.SentDate,
                           opt => opt.MapFrom(src => src.CreationDate.ToLongShamsi()));
        
        CreateMap<Domain.Entities.Ticket, GetTicketDetailsAdminResponse>()
            .ForMember(dest => dest.Messages,
                           opt => opt.Ignore())
            .ForMember(dest => dest.SentDate,
                           opt => opt.MapFrom(src => src.CreationDate.ToLongShamsi()));

    }
}
