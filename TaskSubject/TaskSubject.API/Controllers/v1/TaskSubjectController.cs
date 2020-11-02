using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskSubject.API.ModelsInput.v1;
using TaskSubject.API.ModelsOutput.v1;
using TaskSubject.Core.LogEvents;
using TaskSubject.Core.ModelsDTO;
using TaskSubject.Infrastructure.Facades;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskSubject.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TaskSubjectController : ControllerBase
    {
        private readonly ILogger<TaskSubjectController> _logger;
        private readonly IMapper _mapper;
        private readonly ITaskSubjectFacade _taskSubjectFacade;

        public TaskSubjectController(
            ILogger<TaskSubjectController> logger, 
            IMapper mapper, 
            ITaskSubjectFacade taskSubjectFacade)
        {
            _logger = logger;
            _mapper = mapper;
            _taskSubjectFacade = taskSubjectFacade;
            _logger.LogInformation(MyLogEvents.TaskSubjectControllerConstructor, "TaskSubjectController created");
        }

        [HttpGet]
        public IEnumerable<TaskSubjectModel> Get()
        {
            _logger.LogInformation(MyLogEvents.TaskSubjectControllerGet, "Get list Request");
            return _mapper.Map<IEnumerable<TaskSubjectModel>>(_taskSubjectFacade.GetTaskSubject());
        }

        [HttpGet("{id}")]
        public ActionResult<TaskSubjectModel> Get(string id)
        {
            _logger.LogInformation(MyLogEvents.TaskSubjectControllerGet, "Get Request for {id}", id);

            var result = _mapper.Map<TaskSubjectModel>(_taskSubjectFacade.GetTaskSubject(id));

            if (result != null)
            {
                _logger.LogDebug(MyLogEvents.TaskSubjectControllerGet, "Get model: {uid}, {obj}", result.Uid, result.Object);
                return result;
            }

            _logger.LogInformation(MyLogEvents.TaskSubjectControllerGet, "Get not found response");
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskSubjectCreateModel model, ApiVersion apiVersion)
        {
            _logger.LogInformation(MyLogEvents.TaskSubjectControllerPost, "Get Request is Valid");
            var id = await _taskSubjectFacade.CreateTaskSubjectAsync(_mapper.Map<TaskSubjectDto>(model));
            _logger.LogInformation(MyLogEvents.TaskSubjectControllerPost, "Created Object with internal id {id}", id);
            return CreatedAtAction(nameof(Get), new { id = model.Uid, version = apiVersion.ToString() }, model);
        }

        //// PUT api/<TaskSubjectController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<TaskSubjectController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
