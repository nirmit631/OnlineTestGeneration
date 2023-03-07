using OnlineTest.Data;
using OnlineTest.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTest.Model.Repository
{
    public class RTokenRepository : IRTokenRepository
    {
        private readonly OnlineTestContext _context;
        public RTokenRepository(OnlineTestContext context)
        {
            _context = context;
        }

        public bool Add(RToken token)
        {
            _context.RToken.Add(token);
            return _context.SaveChanges() > 0;
        }

        public bool Expire(RToken token)
        {
            _context.RToken.Update(token);
            return _context.SaveChanges() > 0;
        }

        public RToken Get(string refreshToken)
        {
            return _context.RToken.FirstOrDefault(predicate: x => x.RefreshToken == refreshToken);
        }
    }
}
