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


        [HttpPostAttribute("bill")]
        public Guid Bill([FromBody] BillInputModel input)
        {
            var created = new Domain.Bill.Create(CombGuid.NewGuid(), input.Name, input.Description, input.Total);

            _commandProcessor.Process(created);

            return created.StreamId;
        }


        [HttpPost("bill/comment")]
        public Guid AddComment([FromQueryAttribute] Guid BillId, [FromBody] CommentInputModel input)
        {

            var commentAdded = new Domain.Bill.AddComment(BillId, CombGuid.NewGuid(), input.Comment);

            _commandProcessor.Process(commentAdded);

            return commentAdded.CommentId;
        }
    }

    public class CommentInputModel
    {
        public string Comment { get; set; }
    }
}