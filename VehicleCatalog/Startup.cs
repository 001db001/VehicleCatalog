﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using VehicleCatalog.Models;
using VehicleCatalog.Service;
using VehicleCatalog.Service.Models;
using VehicleCatalog.Service.Services;
using VehicleCatalog.Service.Services.Common;
using VehicleCatalog.Service.Repositories;
using VehicleCatalog.Service.Repositories.Common;

namespace VehicleCatalog
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //Autofac (for container builder)
        public IContainer AppContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            #region Context
            services.AddDbContext<ApplicationDbContex>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            #endregion

            #region MVC

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #endregion

            #region AutoMapper

            var configMapper = new AutoMapper.MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfiles()); });
            var mapper = configMapper.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region Autofac (Without ConfigureContainer)

            //Autofac component registration through reflection (Without ConfigureContainer)
            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterType<MakeService>().As<IMakeService>().InstancePerLifetimeScope();
            builder.RegisterType<ModelService>().As<IModelService>().InstancePerLifetimeScope();

            builder.RegisterType<MakeRepository>().As<IMakeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ModelRepository>().As<IModelRepository>().InstancePerLifetimeScope();

            builder.RegisterType<Pagination>().As<IPagination>().InstancePerRequest();
            builder.RegisterType<Sort>().As<ISort>().InstancePerRequest();
            builder.RegisterType<Filter>().As<IFilter>().InstancePerRequest();

            this.AppContainer = builder.Build();
            return new AutofacServiceProvider(this.AppContainer);
            #endregion

        }

        #region AutoFac (With ConfigureContainer)

        /*//Autofac adding a module to container (With ConfigureContainer)

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
        }
        */
        #endregion


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }


    }
}
