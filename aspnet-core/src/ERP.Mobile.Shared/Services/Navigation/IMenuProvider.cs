using System.Collections.Generic;
using MvvmHelpers;
using ERP.Models.NavigationMenu;

namespace ERP.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}