using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ERP
{
    [DependsOn(typeof(ERPXamarinSharedModule))]
    public class ERPXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ERPXamarinAndroidModule).GetAssembly());
        }
    }
}