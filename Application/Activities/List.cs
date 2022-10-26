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
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<List<ActivityDto>>> {}
        // 3:41
        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly ILogger _logger;
            
            private readonly IMapper _mapper; 

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            
            }
            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

            

                return Result<List<ActivityDto>>.Success(activities);
     
            }

        }

    }
}