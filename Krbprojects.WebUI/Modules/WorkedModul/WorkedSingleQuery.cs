using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.Modules.WorkedModul
{
    public class WorkedSingleQuery : IRequest<Worked>
    {
        public long? Id { get; set; }

        public class WorkedSingleQueryHandler : IRequestHandler<WorkedSingleQuery, Worked>
        {
            readonly KrbProjectsBaseDbContext db;
            public WorkedSingleQueryHandler(KrbProjectsBaseDbContext db)
            {
                this.db = db;
            }
            public async Task<Worked> Handle(WorkedSingleQuery request, CancellationToken cancellationToken)
            {
                if (request.Id <= 0 || request.Id == null)
                {
                    return null;
                }

                var work = await db.Workeds
                    .Include(p => p.Images.Where(i => i.DeletedDate == null))
                    .FirstOrDefaultAsync(m => m.Id == request.Id && m.DeletedByUserId == null, cancellationToken);

                return work;
            }
        }
    }
}
