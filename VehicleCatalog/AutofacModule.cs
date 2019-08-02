﻿using Autofac;
using VehicleCatalog.Service;

namespace VehicleCatalog
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Registering UnitOfWork
            builder.Register(c => new UnitOfWork(c.Resolve<ApplicationDbContex>(), c.Resolve<IMakeRepository>(), c.Resolve<IModelRepository>())).As<IUnitOfWork>().InstancePerLifetimeScope();
            //Registering MakeService
            builder.Register(c => new MakeRepository(c.Resolve<ApplicationDbContex>())).As<IMakeRepository>().InstancePerLifetimeScope();
            //Registering ModelService
            builder.Register(c => new ModelRepository(c.Resolve<ApplicationDbContex>())).As<IModelRepository>().InstancePerLifetimeScope();
        }

    }
}
