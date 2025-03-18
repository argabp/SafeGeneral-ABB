using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.Navigations.Queries
{
    public class GetEditNavigationQuery : IRequest<GetEditNavigationDto>
    {
        public int Id { get; set; }
    }

    public class GetEditNavigationQueryHandler : IRequestHandler<GetEditNavigationQuery, GetEditNavigationDto>
    {
        private readonly IDbConnection _db;

        public GetEditNavigationQueryHandler(IDbConnection db)
        {
            _db = db;
        }
        public async Task<GetEditNavigationDto> Handle(GetEditNavigationQuery request, CancellationToken cancellationToken)
        {
            var nav = (await _db.Query<Navigation>($"SELECT * FROM MS_Navigation WHERE NavigationId = {request.Id}")).SingleOrDefault();
            GetEditNavigationDto dto = new GetEditNavigationDto() { NavigationId = nav.NavigationId, Icon = nav.Icon, Text = nav.Text, IsActive = nav.IsActive, RouteId = nav.RouteId, SubNavigationId = new List<int>() };
            var subnav = await _db.Query<Navigation>($"SELECT * FROM MS_Navigation WHERE ParentId = {request.Id}");
            foreach (Navigation _subnavid in subnav) {
                dto.SubNavigationId.Add(_subnavid.NavigationId);
            }
            return dto;



        }


    }
}