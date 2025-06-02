using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.EmailTemplates.Commands
{
    public class EditEmailTemplateCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Body { get; set; }
    }

    public class EditEmailTemplateCommandHandler : IRequestHandler<EditEmailTemplateCommand>
    {
        private readonly IDbContext _context;

        public EditEmailTemplateCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditEmailTemplateCommand request, CancellationToken cancellationToken)
        {
            var emailTemplate = _context.EmailTemplate.FirstOrDefault(w => w.Id == request.Id);

            if (emailTemplate != null)
            {
                emailTemplate.Name = request.Name;
                emailTemplate.Body = request.Body;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new Unit();
        }
    }
}