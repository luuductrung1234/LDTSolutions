using AutoMapper;

// Chatting Domain
using DomainModel = Chatting.Domain;

// Chatting Application Queries
using Representation = Chatting.Application.Queries.Representations;


namespace Chatting.Application.MappingProfiles
{
   public class DomainToRepresentationMappingProfile : Profile
   {
      public DomainToRepresentationMappingProfile()
      {
         CreateMap<DomainModel.ChatMessage, Representation.ChatMessageRepresentation>();
      }
   }
}
