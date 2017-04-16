using System;
using Microsoft.AspNetCore.Mvc;
using NEvilES.Pipeline;
using TiteAz.Api.Models;
using TiteAz.Common;

namespace TiteAz.Api.Controllers
{
    [RouteAttribute("api/[controller]")]
    public class BillsController : Controller
    {
        public readonly IReadFromReadModel _reader;
        public BillsController(PipelineTransaction pipeline, ICommandProcessor commandProccessor, IReadFromReadModel reader)
        {
            _reader = reader;

        }


        [HttpPostAttribute("[Action]")]
        public Guid Bill([FromBody] BillInputModel input)
        {






            return Guid.NewGuid();
        }
    }
}