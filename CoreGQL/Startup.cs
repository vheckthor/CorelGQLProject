using CoreGQL.Data;
using CoreGQL.GraphQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GraphQL.Server.Ui.Voyager;
using CoreGQL.GraphQL.Platforms;
using CoreGQL.GraphQL.Commands;

namespace CoreGQL
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddPooledDbContextFactory<AppDbContext>(options => options.UseSqlServer
			(Configuration.GetConnectionString("coreconstr")));
			services
				.AddGraphQLServer()
				.AddQueryType<Query>()
				.AddType<PlatformType>()
				.AddType<CommandsType>()
				.AddMutationType<Mutation>()
				.AddSubscriptionType<Subscription>()
				.AddFiltering()
				.AddSorting()
				.AddInMemorySubscriptions();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseWebSockets();
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGraphQL();
			});
			app.UseGraphQLVoyager(new VoyagerOptions { 
				GraphQLEndPoint = "/qraphql",
			});
		}
	}
}
