﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolManagment.Core.Bases;
using SchoolManagment.Core.Features.Authorization.Queries.Model;
using SchoolManagment.Core.Features.Authorization.Queries.Response;
using SchoolManagment.Core.Resources;
using SchoolManagment.Data.Entities.Identity;
using SchoolManagment.Services.Abstracts;

namespace SchoolManagment.Core.Features.Authorization.Queries.Handler
{
    public class RoleQueryHandler : ResponseHandler,
        IRequestHandler<GetRolesListQuery, Response<IReadOnlyList<GetRolesListResponse>>>,
        IRequestHandler<GetRoleByIdQuery, Response<GetRolesListResponse>>

    {
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly RoleManager<Role> roleManager;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;

        public RoleQueryHandler(IStringLocalizer<SharedResource> localizer,
                                RoleManager<Role> roleManager,
                                IMapper mapper,
                                IAuthorizationService authorizationService) : base(localizer)
        {
            this.localizer = localizer;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }
        public async Task<Response<IReadOnlyList<GetRolesListResponse>>> Handle(GetRolesListQuery request, CancellationToken cancellationToken)
        {
            var roles = await authorizationService.GetRolesAsync();
            var mappedRoles = mapper.Map<IReadOnlyList<GetRolesListResponse>>(roles);
            return Success(mappedRoles);
        }

        public async Task<Response<GetRolesListResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await authorizationService.GetRoleByIdAsync(request.Id);
            if (role == null)
                return BadRequest<GetRolesListResponse>((string)localizer[SharedResourcesKeys.NotFound]);

            var mappedRole = mapper.Map<GetRolesListResponse>(role);
            return Success(mappedRole);
        }
    }
}
