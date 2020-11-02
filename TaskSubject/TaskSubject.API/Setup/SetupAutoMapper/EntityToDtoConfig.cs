using AutoMapper;
using TaskSubject.Core.ModelsDTO;
using TaskSubject.DataAccess.Entities;

namespace TaskSubject.API.Setup.SetupAutoMapper
{
    public class EntityToDtoConfig
    {
        public class AutoMapping : Profile
        {
            public AutoMapping()
            {
                CreateMap<TaskSubjectEntity, TaskSubjectDto>();
                CreateMap<TaskSubjectDto, TaskSubjectEntity>();
            }
        }
    }
}
