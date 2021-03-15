using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Persistence;
using Serilog;

namespace Api.Background
{
    public sealed class SeedDataHostedService : IHostedService 
    {
        private readonly UserManager<AppUser> usermanager;
        private readonly DataContext context;

        public SeedDataHostedService(
           UserManager<AppUser> usermanager, DataContext context)
       {
            this.usermanager = usermanager;
            this.context = context;
        
        }

        public async  Task StartAsync(CancellationToken cancellationToken)
        {

            Log.Information("Started the seed data worker service");
            await SeedData.SeedActivities(context, usermanager);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}

/*
WorkServices : background services
The idia of application and hosted service 
Asp.net is a hosted service
  Hosted service controls the lifetime of the service
Hosted service will control the service
 1. shutdown
 2. clean up the application

IHost. Using the base has i
The lifetime is managed by the dotnet core application
The worker service will note exist and since it hosted
 
The main applicaiton usually have a 5 second cleanup, the worker service. The host application will just kill the worker process.
All we need to create our own service is to create a class that implement IHostedService
All hosted service are using the resources of the main application.
When you are scalling up the worker services, The background service may also scale up and this is not something that we may want



*/