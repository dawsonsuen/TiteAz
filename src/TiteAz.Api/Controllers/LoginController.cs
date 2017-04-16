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
    public class LoginController : Controller
    {
        private readonly IReadFromReadModel _reader;
        public LoginController(IReadFromReadModel reader)
        {
            _reader = reader;
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            return null;
        }

    }
}
