using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NEvilES.Pipeline;
using TiteAz.Common;

namespace TiteAz.Api.Controllers
{
    [RouteAttribute("api/[Controller]")]
    public class DebtsController : Controller
    {
        private readonly IReadFromReadModel _reader;
        private readonly CommandContext.IUser _user;
        private readonly ICommandProcessor _commandProcessor;
        private readonly PipelineTransaction _pipeline;

        public DebtsController(PipelineTransaction pipeline, ICommandProcessor commandProccessor, IReadFromReadModel reader, CommandContext.IUser user)
        {
            _pipeline = pipeline;
            _commandProcessor = commandProccessor;
            _reader = reader;
            _user = user;
        }

        public IEnumerable<ReadModel.Debt> Index()
            => _reader.Query<ReadModel.Debt>(x => x.CreditUserId == _user.GuidId || x.DebitUserId == _user.GuidId);

        [RouteAttribute("accept")]
        public ReadModel.Debt Accept([FromQuery] Guid DebtId)
        {
            _commandProcessor.Process(new Domain.Debt.Accept(DebtId));

            return _reader.Get<ReadModel.Debt>(DebtId);
        }

    }
}