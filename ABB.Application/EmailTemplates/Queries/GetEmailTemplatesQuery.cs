using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.EmailTemplates.Queries
{
    public class  GetEmailTemplatesQuery : IRequest<List<EmailTemplateDto>>
    {
    }

    public class GetEmailTemplatesQueryHandler : IRequestHandler<GetEmailTemplatesQuery, List<EmailTemplateDto>>
    {
        private readonly IDbConnection _dbConnection;

        public GetEmailTemplatesQueryHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<EmailTemplateDto>> Handle(GetEmailTemplatesQuery request, CancellationToken cancellationToken)
        {
            var emailTemplates = 
                (await _dbConnection.Query<EmailTemplateDto>(@"SELECT et.Id, et.Name, et.Body FROM MS_EmailTemplate et")).ToList();

            return emailTemplates;
        }
    }
}