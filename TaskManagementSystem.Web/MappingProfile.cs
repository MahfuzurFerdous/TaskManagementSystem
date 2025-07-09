using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping between TaskCard entity and TaskCardDto
            CreateMap<TaskCard, TaskCardDto>()
                // Map approval flags explicitly
                .ForMember(dest => dest.IsManagerApproved, opt => opt.MapFrom(src => src.IsManagerApproved))
                .ForMember(dest => dest.IsAdminApproved, opt => opt.MapFrom(src => src.IsAdminApproved))
                .ReverseMap(); // For reverse mapping (Dto -> Entity)

            // Mapping between TaskCardDto and TaskCardViewModel
            CreateMap<TaskCardDto, TaskCardViewModel>()
                .ForMember(dest => dest.IsManagerApproved, opt => opt.MapFrom(src => src.IsManagerApproved))
                .ForMember(dest => dest.IsAdminApproved, opt => opt.MapFrom(src => src.IsAdminApproved))
                .ForMember(dest => dest.IsRequestedForCompletion, opt => opt.MapFrom(src => src.IsRequestedForCompletion))
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ReverseMap();

            // Direct mapping between TaskCard entity and TaskCardViewModel if needed
            CreateMap<TaskCard, TaskCardViewModel>()
                .ForMember(dest => dest.IsManagerApproved, opt => opt.MapFrom(src => src.IsManagerApproved))
                .ForMember(dest => dest.IsAdminApproved, opt => opt.MapFrom(src => src.IsAdminApproved))
                .ForMember(dest => dest.IsRequestedForCompletion, opt => opt.MapFrom(src => src.IsRequestedForCompletion))
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ReverseMap();

            // Mapping for CreateTaskCard ViewModel to DTO (ignore CreatedByUserName since it's set manually)
            CreateMap<CreateTaskCardViewModel, CreateTaskCardDto>()
                .ForMember(dest => dest.CreatedByUserName, opt => opt.Ignore());
            CreateMap<CreateTaskCardDto, CreateTaskCardViewModel>().ReverseMap();


            // Mapping User DTOs and ViewModels
            CreateMap<UserDto, UserViewModel>().ReverseMap();

            // Mapping SelectListItem (needed for dropdowns)
            CreateMap<SelectListItem, SelectListItem>().ReverseMap();

            // Mapping EditTaskCard models and DTOs
            CreateMap<EditTaskCardViewModel, EditTaskCardDto>().ReverseMap();
            CreateMap<EditTaskCardDto, TaskCard>().ReverseMap();
            CreateMap<TaskCard, EditTaskCardViewModel>().ReverseMap();

            // Mapping between AssignToUser ViewModel and DTO
            CreateMap<AssignToUserViewModel, AssignToUserViewModelDto>().ReverseMap();

            // Mapping from TaskCard to AssignToUserViewModel, ignore AvailableUsers because it's populated manually
            CreateMap<TaskCard, AssignToUserViewModel>()
                .ForMember(dest => dest.AvailableUsers, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedToUserName, opt => opt.MapFrom(src => src.AssignedToUserName))
                .ForMember(dest => dest.TaskCardId, opt => opt.MapFrom(src => src.Id));

            // Mapping TaskCard to UserTaskCardDto and vice versa (if you use these)
            CreateMap<TaskCard, UserTaskCardDto>().ReverseMap();
            CreateMap<UserTaskCardDto, UserTaskCardViewModel>().ReverseMap();

            // Mapping UserProfile DTOs and ViewModels
            CreateMap<UserProfileViewModel, UserProfileDto>().ReverseMap();

            // Mapping TaskCardList models and DTOs
            CreateMap<TaskCardListViewModel, TaskCardListViewDto>().ReverseMap();

            // Mapping TaskStandupLog models and DTOs
            CreateMap<TaskStandupLog, TaskStandupLogDto>().ReverseMap();
            CreateMap<TaskStandupLogDto, TaskStandupLogViewModel>().ReverseMap();
        }
    }
}
