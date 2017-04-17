using System;
using System.Collections.Generic;
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

        public IEnumerable<ReadModel.Bill> Index() => _reader.Query<ReadModel.Bill>(x => true);

        [HttpPost("bill")]
        public Guid Bill([FromBody] BillInputModel input)
        {
            var created = new Domain.Bill.Create(CombGuid.NewGuid(), input.Name, input.Description, input.Total);

            _commandProcessor.Process(created);

            return created.StreamId;
        }


        [HttpPost("bill/comment")]
        public Guid AddComment([FromQueryAttribute] Guid BillId, [FromBody] NewCommentInputModel input)
        {

            var commentAdded = new Domain.Bill.AddComment(BillId, CombGuid.NewGuid(), input.Comment);

            _commandProcessor.Process(commentAdded);

            return commentAdded.CommentId;
        }

        [HttpPatchAttribute("bill/comment")]
        public Guid EditComment([FromQueryAttribute] Guid BillId, [FromBody] EditCommentInputModel input)
        {

            var commentAdded = new Domain.Bill.EditComment(BillId, input.CommentId, input.Comment);

            _commandProcessor.Process(commentAdded);

            return commentAdded.CommentId;
        }

        [HttpDeleteAttribute("bill/comment")]
        public Guid RemoveComment([FromQueryAttribute] Guid BillId, [FromBody] Guid commentId)
        {

            var commentAdded = new Domain.Bill.RemoveComment(BillId, commentId);

            _commandProcessor.Process(commentAdded);

            return commentAdded.CommentId;
        }
    }

    public class NewCommentInputModel
    {
        public string Comment { get; set; }
    }

    public class EditCommentInputModel
    {
        public Guid CommentId { get; set; }
        public string Comment { get; set; }
    }
}