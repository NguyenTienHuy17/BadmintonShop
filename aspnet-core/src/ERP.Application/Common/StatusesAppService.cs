using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Common.Exporting;
using ERP.Common.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;

namespace ERP.Common
{
    [AbpAuthorize(AppPermissions.Pages_Statuses)]
    public class StatusesAppService : ERPAppServiceBase, IStatusesAppService
    {
        private readonly IRepository<Status, long> _statusRepository;
        private readonly IStatusesExcelExporter _statusesExcelExporter;

        public StatusesAppService(IRepository<Status, long> statusRepository, IStatusesExcelExporter statusesExcelExporter)
        {
            _statusRepository = statusRepository;
            _statusesExcelExporter = statusesExcelExporter;

        }

        public async Task<PagedResultDto<GetStatusForViewDto>> GetAll(GetAllStatusesInput input)
        {

            var filteredStatuses = _statusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredStatuses = filteredStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var statuses = from o in pagedAndFilteredStatuses
                           select new
                           {

                               o.Name,
                               o.Description,
                               Id = o.Id
                           };

            var totalCount = await filteredStatuses.CountAsync();

            var dbList = await statuses.ToListAsync();
            var results = new List<GetStatusForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStatusForViewDto()
                {
                    Status = new StatusDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStatusForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStatusForViewDto> GetStatusForView(long id)
        {
            var status = await _statusRepository.GetAsync(id);

            var output = new GetStatusForViewDto { Status = ObjectMapper.Map<StatusDto>(status) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Statuses_Edit)]
        public async Task<GetStatusForEditOutput> GetStatusForEdit(EntityDto<long> input)
        {
            var status = await _statusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStatusForEditOutput { Status = ObjectMapper.Map<CreateOrEditStatusDto>(status) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStatusDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Statuses_Create)]
        protected virtual async Task Create(CreateOrEditStatusDto input)
        {
            var status = ObjectMapper.Map<Status>(input);

            if (AbpSession.TenantId != null)
            {
                status.TenantId = (int?)AbpSession.TenantId;
            }

            await _statusRepository.InsertAsync(status);

        }

        [AbpAuthorize(AppPermissions.Pages_Statuses_Edit)]
        protected virtual async Task Update(CreateOrEditStatusDto input)
        {
            var status = await _statusRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, status);

        }

        [AbpAuthorize(AppPermissions.Pages_Statuses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _statusRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStatusesToExcel(GetAllStatusesForExcelInput input)
        {

            var filteredStatuses = _statusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var query = (from o in filteredStatuses
                         select new GetStatusForViewDto()
                         {
                             Status = new StatusDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });

            var statusListDtos = await query.ToListAsync();

            return _statusesExcelExporter.ExportToFile(statusListDtos);
        }

        public async Task<List<GetStatusForViewDto>> GetAllStatusForSelect()
        {

            var filteredStatuses = await _statusRepository.GetAllListAsync();

            var statuses = from o in filteredStatuses
                           select new
                           {
                               o.Name,
                               o.Description,
                               Id = o.Id
                           };
            var results = new List<GetStatusForViewDto>();

            foreach (var o in statuses)
            {
                var res = new GetStatusForViewDto()
                {
                    Status = new StatusDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return results;
        }

    }
}