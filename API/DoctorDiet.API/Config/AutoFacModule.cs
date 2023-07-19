using Autofac;
using DoctorDiet.Data;
using DoctorDiet.Repositories.Interfaces;
using DoctorDiet.Repositories.Repositories;
using DoctorDiet.Repository.Interfaces;
using DoctorDiet.Repository.Reposetories;
using DoctorDiet.Repository.Repositories;
using DoctorDiet.Repository.UnitOfWork;
using DoctorDiet.Services;

namespace DoctorDiet.API.Config
{
    public class AutoFacModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(Context)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(GenericRepository<,>)).As(typeof(IGenericRepository<,>)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(AccountRepository)).As(typeof(IAccountRepository)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(AdminService).Assembly).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(DoctorService).Assembly).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(PatientService).Assembly).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(NoteService).Assembly).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(CustomPlanService).Assembly).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(ContactInfoService).Assembly).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(AccountService).Assembly).InstancePerLifetimeScope();

        }
    }
}
