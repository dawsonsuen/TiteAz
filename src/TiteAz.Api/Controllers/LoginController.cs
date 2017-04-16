using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NEvilES.Pipeline;
using TiteAz.Api.Models;
using TiteAz.Common;
using TiteAz.ReadModel;

namespace TiteAz.Api.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IReadFromReadModel _reader;
        public LoginController(IReadFromReadModel reader)
        {
            _reader = reader;
        }

        [HttpPost]

        public IActionResult Login([FromBody]LoginInputModel model)
        {
            return Json(((SqlReadModel)_reader).Query<User>(x => x.Email == model.Username).FirstOrDefault());
        }
    }
}
