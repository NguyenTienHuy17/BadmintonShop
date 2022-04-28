using ERP.Purchase.Dtos;
using ERP.Purchase;
using ERP.Entity.Dtos;
using ERP.Entity;
using ERP.Common.Dtos;
using ERP.Common;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using AutoMapper;
using ERP.Auditing.Dto;
using ERP.Authorization.Accounts.Dto;
using ERP.Authorization.Permissions.Dto;
using ERP.Authorization.Roles;
using ERP.Authorization.Roles.Dto;
using ERP.Authorization.Users;
using ERP.Authorization.Users.Dto;
using ERP.Authorization.Users.Importing.Dto;
using ERP.Authorization.Users.Profile.Dto;
using ERP.Chat;
using ERP.Chat.Dto;
using ERP.Editions;
using ERP.Editions.Dto;
using ERP.Friendships;
using ERP.Friendships.Cache;
using ERP.Friendships.Dto;
using ERP.Localization.Dto;
using ERP.MultiTenancy;
using ERP.MultiTenancy.Dto;
using ERP.MultiTenancy.HostDashboard.Dto;
using ERP.MultiTenancy.Payments;
using ERP.MultiTenancy.Payments.Dto;
using ERP.Notifications.Dto;
using ERP.Organizations.Dto;
using ERP.Sessions.Dto;

namespace ERP
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditCartDto, Cart>().ReverseMap();
            configuration.CreateMap<CartDto, Cart>().ReverseMap();
            configuration.CreateMap<CreateOrEditReturnProdDto, ReturnProd>().ReverseMap();
            configuration.CreateMap<ReturnProdDto, ReturnProd>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderItemDto, OrderItem>().ReverseMap();
            configuration.CreateMap<OrderItemDto, OrderItem>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderDto, Order>().ReverseMap();
            configuration.CreateMap<OrderDto, Order>().ReverseMap();
            configuration.CreateMap<CreateOrEditStatusDto, Status>().ReverseMap();
            configuration.CreateMap<StatusDto, Status>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductImageDto, ProductImage>().ReverseMap();
            configuration.CreateMap<ProductImageDto, ProductImage>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductDto, Product>().ReverseMap();
            configuration.CreateMap<ProductDto, Product>().ReverseMap();
            configuration.CreateMap<ProductDto, Product>();
            configuration.CreateMap<CreateOrEditCategoryDto, Category>().ReverseMap();
            configuration.CreateMap<CategoryDto, Category>().ReverseMap();
            configuration.CreateMap<CreateOrEditBrandDto, Brand>().ReverseMap();
            configuration.CreateMap<BrandDto, Brand>().ReverseMap();
            configuration.CreateMap<CreateOrEditDiscountDto, Discount>().ReverseMap();
            configuration.CreateMap<DiscountDto, Discount>().ReverseMap();
            configuration.CreateMap<CreateOrEditBlogDto, Blog>().ReverseMap();
            configuration.CreateMap<BlogDto, Blog>().ReverseMap();
            configuration.CreateMap<CreateOrEditBookingDto, Booking>().ReverseMap();
            configuration.CreateMap<BookingDto, Booking>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();

            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}