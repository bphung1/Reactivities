using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain;
using Reactivities.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Activities
{
    public class List
    {
        public class Query : IRequest<List<Activity>> { }

        public class Handler : IRequestHandler<Query, List<Activity>>
        {
            private readonly ReactivitiesDbContext _context;
            public Handler(ReactivitiesDbContext context)
            {
                _context = context;
            }

            public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
                => await _context.Activities.ToListAsync();
        }
    }
}
