using System;
using Microsoft.AspNetCore.Mvc;
using NEvilES;
using NEvilES.Pipeline;
using TiteAz.Api.Models;
using TiteAz.Common;

namespace TiteAz.Api.Controllers
{
    [RouteAttribute("api/[controller]")]
    public class BillsController : Controller
    {
        public readonly IReadFromReadModel _reader;
        private readonly PipelineTransaction _pipeline;
        private readonly ICommandProcessor _commandProcessor;

        public BillsController(PipelineTransaction pipeline, ICommandProcessor commandProccessor, IReadFromReadModel reader)
        {
            _reader = reader;
            _pipeline = pipeline;
            _commandProcessor = commandProccessor;
        }


        [HttpPostAttribute("[Action]")]
        public Guid Bill([FromBody] BillInputModel input)
        {
            var created = new Domain.Bill.Created()
            {
                StreamId = CombGuid.NewGuid(),
                Name = input.Name,
                Description = input.Description,
                Amount = input.Total
            };

            _commandProcessor.Process(created);





            return created.StreamId;
        }
    }
}