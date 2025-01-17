﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Reactivities.Application.Core;
using Reactivities.Domain;
using Reactivities.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Activities
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly ReactivitiesDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ReactivitiesDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Activity.Id);

                if (activity == null) 
                    return null;

                _mapper.Map(request.Activity, activity);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) 
                    return Result<Unit>.Failure("Failed to update the activity");


                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
