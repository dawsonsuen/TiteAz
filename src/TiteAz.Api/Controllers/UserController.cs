using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using TiteAz.Common;
using TiteAz.ReadModel;

namespace TiteAz.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly SqlConnection _sqlConn;
        public readonly IReadData _reader;
        public UserController(SqlConnection sqlConn, IReadData reader)
        {
            _reader = reader;
            _sqlConn = sqlConn;
        }

        [HttpGet("[action]")]
        public IEnumerable<TiteAz.ReadModel.Debt> Debts(GetDebts.QueryModel query)
        {
            var list = new List<TiteAz.ReadModel.Debt>();

            var cmd = _sqlConn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select id, bill_id, debit_user_id, credit_user_id, amount, debt_status from debts";
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    list.Add(new ReadModel.Debt
                    {
                        Id = r.GetGuid(0),
                        BillId = r.GetGuid(1),
                        DebitUserId = r.GetGuid(2),
                        CreditUserId = r.GetGuid(3),
                        Amount = r.GetDecimal(4),
                        Status = r.GetString(5)

                    });
                }
            }

            return list;
        }

        [HttpGetAttribute("[Action]")]
        public User GetUser(string email)
        {
            Guid userId = Guid.Empty;

            var cmd = _sqlConn.CreateCommand();
            cmd.CommandText = "select id from users where :email";
            cmd.Parameters.AddWithValue(":email", email);
            using (var r = cmd.ExecuteReader())
            {
                r.Read();
                userId = r.GetGuid(0);
            }


            var user = _reader.Get<User>(userId);
            return user;
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
