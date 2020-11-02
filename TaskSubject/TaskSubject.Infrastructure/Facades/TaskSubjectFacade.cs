using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TaskSubject.Core.LogEvents;
using TaskSubject.Core.ModelsDTO;
using TaskSubject.DataAccess.Entities;
using TaskSubject.DataAccess.Infrastructure;
using TaskSubject.DataAccess.TaskSubject;

namespace TaskSubject.Infrastructure.Facades
{
    public class TaskSubjectFacade : ITaskSubjectFacade
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ITaskSubjectCommand _sampleTaskDataAccess;

        public TaskSubjectFacade(
            ILogger<TaskSubjectFacade> logger, 
            IMapper mapper, 
            ITaskSubjectCommand sampleTaskDataAccess)
        {
            _logger = logger;
            _mapper = mapper;
            _sampleTaskDataAccess = sampleTaskDataAccess;
        }

        public async Task<string> CreateTaskSubjectAsync(TaskSubjectDto model)
        {
            _logger.LogInformation(MyLogEvents.TaskSubjectGetFacade, "Facade call: CreateTaskSubjectAsync");
            return await _sampleTaskDataAccess.Create(_mapper.Map<TaskSubjectEntity>(model));
        }

        public IEnumerable<TaskSubjectDto> GetTaskSubject()
        {
            _logger.LogInformation(MyLogEvents.TaskSubjectsGetFacade, "Facade call: GetTaskSubject");
            return _mapper.Map<IEnumerable<TaskSubjectDto>>(_sampleTaskDataAccess.List());

        }

        public TaskSubjectDto GetTaskSubject(string uid)
        {
            _logger.LogInformation(MyLogEvents.TaskSubjectCreateFacade, "Facade call: GetTaskSubject");
            return _mapper.Map<TaskSubjectDto>(_sampleTaskDataAccess.Get(uid));
        }
    }
}
