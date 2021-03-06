using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Threading;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ERP.Authorization.Roles;
using ERP.Security;

namespace ERP.Authorization.Users
{
    /// <summary>
    /// User manager.
    /// Used to implement domain logic for users.
    /// Extends <see cref="AbpUserManager{TRole,TUser}"/>.
    /// </summary>
    public class UserManager : AbpUserManager<Role, User>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly ISettingManager _settingManager;
        private readonly IRepository<User, long> _userRepository;

        public UserManager(
            UserStore userStore,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager> logger,
            RoleManager roleManager,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            ISettingManager settingManager, 
            ILocalizationManager localizationManager,
            IRepository<User, long> userRepository)
            : base(
                  roleManager,
                  userStore,
                  optionsAccessor,
                  passwordHasher,
                  userValidators,
                  passwordValidators,
                  keyNormalizer,
                  errors,
                  services,
                  logger,
                  permissionManager,
                  unitOfWorkManager,
                  cacheManager,
                  organizationUnitRepository,
                  userOrganizationUnitRepository,
                  organizationUnitSettings,
                  settingManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _settingManager = settingManager;
            _localizationManager = localizationManager;
            _userRepository = userRepository;
        }

        [UnitOfWork]
        public virtual async Task<User> GetUserOrNullAsync(UserIdentifier userIdentifier)
        {
            using (_unitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                return await FindByIdAsync(userIdentifier.UserId.ToString());
            }
        }

        public User GetUserOrNull(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserOrNullAsync(userIdentifier));
        }

        public async Task<User> GetUserAsync(UserIdentifier userIdentifier)
        {
            var user = await GetUserOrNullAsync(userIdentifier);
            if (user == null)
            {
                throw new Exception("There is no user: " + userIdentifier);
            }

            return user;
        }

        public User GetUser(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserAsync(userIdentifier));
        }

        public override Task<IdentityResult> SetRoles(User user, string[] roleNames)
        {
            if (user.Name == "admin" && !roleNames.Contains(StaticRoleNames.Host.Admin))
            {
                throw new UserFriendlyException(L("AdminRoleCannotRemoveFromAdminUser"));
            }

            return base.SetRoles(user, roleNames);
        }

        public override async Task SetGrantedPermissionsAsync(User user, IEnumerable<Permission> permissions)
        {
            CheckPermissionsToUpdate(user, permissions);

            await base.SetGrantedPermissionsAsync(user, permissions);
        }

        public async Task<string> CreateRandomPassword()
        {
            var passwordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit = await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit),
                RequireLowercase = await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric = await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase = await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase),
                RequiredLength = await _settingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength)
            };

            var upperCaseLetters = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            var lowerCaseLetters = "abcdefghijkmnopqrstuvwxyz";
            var digits = "0123456789";
            var nonAlphanumerics = "!@$?_-";

            string[] randomChars = {
                upperCaseLetters,
                lowerCaseLetters,
                digits,
                nonAlphanumerics
            };

            var rand = new Random(Environment.TickCount);
            var chars = new List<char>();

            if (passwordComplexitySetting.RequireUppercase)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    upperCaseLetters[rand.Next(0, upperCaseLetters.Length)]);
            }

            if (passwordComplexitySetting.RequireLowercase)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    lowerCaseLetters[rand.Next(0, lowerCaseLetters.Length)]);
            }

            if (passwordComplexitySetting.RequireDigit)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    digits[rand.Next(0, digits.Length)]);
            }

            if (passwordComplexitySetting.RequireNonAlphanumeric)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    nonAlphanumerics[rand.Next(0, nonAlphanumerics.Length)]);
            }

            for (var i = chars.Count; i < passwordComplexitySetting.RequiredLength; i++)
            {
                var rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        private void CheckPermissionsToUpdate(User user, IEnumerable<Permission> permissions)
        {
            if (user.Name == AbpUserBase.AdminUserName &&
                (!permissions.Any(p => p.Name == AppPermissions.Pages_Administration_Roles_Edit) ||
                !permissions.Any(p => p.Name == AppPermissions.Pages_Administration_Users_ChangePermissions)))
            {
                throw new UserFriendlyException(L("YouCannotRemoveUserRolePermissionsFromAdminUser"));
            }
        }

        private new string L(string name)
        {
            return _localizationManager.GetString(ERPConsts.LocalizationSourceName, name);
        }

        public async Task CreateUserAsync(User input)
        {
            input.NormalizedEmailAddress = input.EmailAddress.ToUpper();
            input.NormalizedUserName = input.UserName.ToUpper();
            var user = await _userRepository.GetAllListAsync();

            var tempName = user.FirstOrDefault(i => i.PhoneNumber == input.PhoneNumber);
            if (tempName != null)
            {
                throw new UserFriendlyException(L("Identity.DuplicatePhoneNumber"));
            }
            var tempEmail = user.FirstOrDefault(i => i.EmailAddress == input.EmailAddress);

            if (tempEmail != null)
            {
                throw new UserFriendlyException(L("Identity.DuplicateEmail"));
            }
            var tempUserName = user.FirstOrDefault(i => i.UserName == input.UserName);
            if (tempUserName != null)
            {
                throw new UserFriendlyException(L("Identity.DuplicateUserName"));
            }
            try
            {
                await _userRepository.InsertAsync(input);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("CannotCreateNewUser"));
            }
        }

        public async Task UpdateUserAsync(User input)
        {
            input.NormalizedEmailAddress = input.EmailAddress.ToUpper();
            input.NormalizedUserName = input.UserName.ToUpper();
            var userCheck = await _userRepository.GetAllListAsync();

            var tempName = userCheck.FirstOrDefault(i => i.PhoneNumber == input.PhoneNumber && i.Id != input.Id);
            if (tempName != null)
            {
                throw new UserFriendlyException(L("Identity.DuplicatePhoneNumber"));
            }
            var tempEmail = userCheck.FirstOrDefault(i => i.EmailAddress == input.EmailAddress && i.Id != input.Id);

            if (tempEmail != null)
            {
                throw new UserFriendlyException(L("Identity.DuplicateEmail"));
            }
            var tempUserName = userCheck.FirstOrDefault(i => i.UserName == input.UserName && i.Id != input.Id);
            if (tempUserName != null)
            {
                throw new UserFriendlyException(L("Identity.DuplicateUserName"));
            }
            try
            {
                var user = await _userRepository.FirstOrDefaultAsync((long)input.Id);
                user = input;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("CannotCreateNewUser"));
            }
        }
    }
}