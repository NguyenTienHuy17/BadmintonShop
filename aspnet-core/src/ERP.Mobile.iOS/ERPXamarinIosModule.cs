using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ERP
{
    [DependsOn(typeof(ERPXamarinSharedModule))]
    public class ERPXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ERPXamarinIosModule).GetAssembly());
        }
    }
}