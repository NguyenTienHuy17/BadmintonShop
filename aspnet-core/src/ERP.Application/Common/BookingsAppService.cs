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
using Abp.Runtime.Session;

namespace ERP.Common
{
    public class BookingsAppService : ERPAppServiceBase, IBookingsAppService
    {
        private readonly IRepository<Booking, long> _bookingRepository;
        private readonly IBookingsExcelExporter _bookingsExcelExporter;
        private readonly IAbpSession _abpSession;

        public BookingsAppService(IRepository<Booking, long> bookingRepository, 
            IBookingsExcelExporter bookingsExcelExporter,
            IAbpSession abpSession)
        {
            _bookingRepository = bookingRepository;
            _bookingsExcelExporter = bookingsExcelExporter;
            _abpSession = abpSession;
        }

        public async Task<PagedResultDto<GetBookingForViewDto>> GetAll(GetAllBookingsInput input)
        {

            var filteredBookings = _bookingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinTimeFilter != null, e => e.Time >= input.MinTimeFilter)
                        .WhereIf(input.MaxTimeFilter != null, e => e.Time <= input.MaxTimeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredBookings = filteredBookings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var bookings = from o in pagedAndFilteredBookings
                           select new
                           {

                               o.Time,
                               o.Description,
                               Id = o.Id
                           };

            var totalCount = await filteredBookings.CountAsync();

            var dbList = await bookings.ToListAsync();
            var results = new List<GetBookingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBookingForViewDto()
                {
                    Booking = new BookingDto
                    {

                        Time = o.Time,
                        Description = o.Description,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBookingForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBookingForViewDto> GetBookingForView(long id)
        {
            var booking = await _bookingRepository.GetAsync(id);

            var output = new GetBookingForViewDto { Booking = ObjectMapper.Map<BookingDto>(booking) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Bookings_Edit)]
        public async Task<GetBookingForEditOutput> GetBookingForEdit(EntityDto<long> input)
        {
            var booking = await _bookingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBookingForEditOutput { Booking = ObjectMapper.Map<CreateOrEditBookingDto>(booking) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBookingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Bookings_Create)]
        protected virtual async Task Create(CreateOrEditBookingDto input)
        {
            var booking = ObjectMapper.Map<Booking>(input);

            if (AbpSession.TenantId != null)
            {
                booking.TenantId = (int?)AbpSession.TenantId;
            }

            await _bookingRepository.InsertAsync(booking);

        }

        [AbpAuthorize(AppPermissions.Pages_Bookings_Edit)]
        protected virtual async Task Update(CreateOrEditBookingDto input)
        {
            var booking = await _bookingRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, booking);

        }

        [AbpAuthorize(AppPermissions.Pages_Bookings_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _bookingRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBookingsToExcel(GetAllBookingsForExcelInput input)
        {

            var filteredBookings = _bookingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinTimeFilter != null, e => e.Time >= input.MinTimeFilter)
                        .WhereIf(input.MaxTimeFilter != null, e => e.Time <= input.MaxTimeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var query = (from o in filteredBookings
                         select new GetBookingForViewDto()
                         {
                             Booking = new BookingDto
                             {
                                 Time = o.Time,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });

            var bookingListDtos = await query.ToListAsync();

            return _bookingsExcelExporter.ExportToFile(bookingListDtos);
        }

        public async Task<PagedResultDto<GetBookingForViewDto>> GetAllForUser()
        {

            var filteredBookings = _bookingRepository.GetAll().Where(x => x.CreatorUserId == _abpSession.UserId);

            var pagedAndFilteredBookings = filteredBookings
                .OrderBy(x => x.CreationTime);

            var bookings = from o in pagedAndFilteredBookings
                           select new
                           {

                               o.Time,
                               o.Description,
                               Id = o.Id
                           };

            var totalCount = await filteredBookings.CountAsync();

            var dbList = await bookings.ToListAsync();
            var results = new List<GetBookingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBookingForViewDto()
                {
                    Booking = new BookingDto
                    {

                        Time = o.Time,
                        Description = o.Description,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBookingForViewDto>(
                totalCount,
                results
            );

        }
    }
}