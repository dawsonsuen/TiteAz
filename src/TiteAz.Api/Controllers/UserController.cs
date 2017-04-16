using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using NEvilES.Pipeline;
using TiteAz.Common;
using TiteAz.ReadModel;

namespace TiteAz.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        public readonly IReadFromReadModel _reader;
        public UserController(IReadFromReadModel reader)
        {
            _reader = reader;

        }

        // [HttpGet("[action]")]
        // public IEnumerable<TiteAz.ReadModel.Debt> Debts(GetDebts.QueryModel query)
        // {



        //     return list;
        // }

        [HttpGetAttribute("[Action]")]
        public ReadModel.User GetUser(Guid id)
        {
            var user = _reader.Get<ReadModel.User>(id);

            return user;
        }

        //This should be used to upload attachments for bills so that they exist before bill exists
        public IActionResult AddToGallery()
        {

            return Json("");
        }
    }


    public class GetDebts
    {
        public class QueryModel
        {
            public string Email { get; set; }
        }
    }
}
