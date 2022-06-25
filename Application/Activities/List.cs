using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using System.Collections.Generic;
using Domain;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<List<Activity>> {}
        // 3:41
        public class Handler : IRequestHandler<Query, List<Activity>>
        {
            private readonly DataContext _context;
            private readonly ILogger _logger;
            // 6/17 3:37

            public Handler(DataContext context)
            {
                _context = context;
            
            }
            public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                
                return await _context.Activities.ToListAsync();
                // 5:34
            }

        }

    }
}