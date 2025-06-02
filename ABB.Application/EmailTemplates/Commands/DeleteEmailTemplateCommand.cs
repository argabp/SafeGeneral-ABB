using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.EmailTemplates.Commands
{
    public class DeleteEmailTemplateCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteEmailTemplateCommandHandler : IRequestHandler<DeleteEmailTemplateCommand>
    {
        private readonly IDbContext _context;

        public DeleteEmailTemplateCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteEmailTemplateCommand request, CancellationToken cancellationToken)
        {
            var emailTemplate = _context.EmailTemplate.FirstOrDefault(w => w.Id == request.Id);

            if (emailTemplate != null) _context.EmailTemplate.Remove(emailTemplate);

            await _context.SaveChangesAsync(cancellationToken);

            return new Unit();
        }
    }
}