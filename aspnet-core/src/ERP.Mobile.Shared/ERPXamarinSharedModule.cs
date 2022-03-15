using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ERP
{
    [DependsOn(typeof(ERPClientModule), typeof(AbpAutoMapperModule))]
    public class ERPXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ERPXamarinSharedModule).GetAssembly());
        }
    }
}