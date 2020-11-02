using AutoMapper;
using TaskSubject.API.ModelsInput.v1;
using TaskSubject.API.ModelsOutput.v1;
using TaskSubject.Core.ModelsDTO;

namespace TaskSubject.API.Setup.SetupAutoMapper
{
    public class ApiToDtoConfig
    {
        public class AutoMapping : Profile
        {
            public AutoMapping()
            {
                CreateMap<TaskSubjectCreateModel, TaskSubjectDto>();
                CreateMap<TaskSubjectDto, TaskSubjectModel>();
            }
        }
    }
}
