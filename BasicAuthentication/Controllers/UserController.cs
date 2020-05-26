using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenricationDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicAuthentication.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly CommonDbContext _commonDb;

        public UserController(CommonDbContext commonDb)
        {
            _commonDb = commonDb;
        }

        [HttpGet]
        public async Task<object> Value()
        {
            return await _commonDb.Users.ToListAsync();
        }
    }
}
