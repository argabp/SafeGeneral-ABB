using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.EmailTemplates.Queries
{
    public class  GetEmailTemplateQuery : IRequest<EmailTemplate>
    {
        public int Id { get; set; }
    }

    public class GetEmailTemplateQueryHandler : IRequestHandler<GetEmailTemplateQuery, EmailTemplate>
    {
        private readonly IDbContext _context;

        public GetEmailTemplateQueryHandler(IDbContext context)
        {
            _context = context;
        }

        public Task<EmailTemplate> Handle(GetEmailTemplateQuery request, CancellationToken cancellationToken)
        {
            var emailTemplate = _context.EmailTemplate.FirstOrDefault(w => w.Id == request.Id);

            return Task.FromResult(emailTemplate);
        }
    }
}