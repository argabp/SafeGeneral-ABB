using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.EmailTemplates.Commands
{
    public class AddEmailTemplateCommand : IRequest
    {
        public string Name { get; set; }

        public string Body { get; set; }
    }

    public class AddEmailTemplateCommandHandler : IRequestHandler<AddEmailTemplateCommand>
    {
        private readonly IDbContext _context;

        public AddEmailTemplateCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddEmailTemplateCommand request, CancellationToken cancellationToken)
        {
            var emailTemplate = new EmailTemplate()
            {
                Name = request.Name,
                Body = request.Body
            };

            _context.EmailTemplate.Add(emailTemplate);
            
            await _context.SaveChangesAsync(cancellationToken);

            return new Unit();
        }
    }
}