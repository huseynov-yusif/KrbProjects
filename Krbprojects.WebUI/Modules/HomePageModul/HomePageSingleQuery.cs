

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.Modules.HomePageModul
{
    public class HomePageSingleQuery : IRequest<HomePage>
    {
        public long? Id { get; set; }

        public class HomePageSingleQueryHandler : IRequestHandler<HomePageSingleQuery, HomePage>
        {
            readonly KrbProjectsBaseDbContext db;
            public HomePageSingleQueryHandler(KrbProjectsBaseDbContext db)
            {
                this.db = db;
            }
            public async Task<HomePage> Handle(HomePageSingleQuery request, CancellationToken cancellationToken)
            {
                if (request.Id <= 0 || request.Id == null)
                {
                    return null;
                }

                var home = await db.HomePages
                    .Include(p => p.Images.Where(i => i.DeletedDate == null))
                    .FirstOrDefaultAsync(m => m.Id == request.Id && m.DeletedByUserId == null, cancellationToken);

                return home;
            }
        }
    }
}