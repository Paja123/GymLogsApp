using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Base
{
    public abstract class AuthorizedHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected readonly ICurrentUserService CurrentUserService;
        
        protected AuthorizedHandler(ICurrentUserService currentUserService)
        {
            CurrentUserService = currentUserService;
        }

        protected string getUserId()
            => CurrentUserService.UserId ?? throw new UnauthorizedAccessException("User is not authenticated.");

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
       
    }
}
