using CoreGQL.Data;
using CoreGQL.Models;
using HotChocolate;
using HotChocolate.Types;
using System.Linq;

namespace CoreGQL.GraphQL.Platforms
{
	public class PlatformType : ObjectType<Platform>
	{
		protected override void Configure(IObjectTypeDescriptor<Platform> descriptor)
		{
			descriptor.Description("The commandline commands for different actions on the command line interface");
			descriptor.Field(p => p.LicenseKey).Ignore(); 
			descriptor
				.Field(x => x.Commands)
				.ResolveWith<Resolvers>(p => p.GetCommands(default!, default!))
				.UseDbContext<AppDbContext>()
				.Description("This is the list of available commands for this platform");
		}

		private class Resolvers
		{
			public IQueryable<Command> GetCommands(Platform platform, [ScopedService] AppDbContext context)
			{
				return context.Commands.Where(c => c.PlatformId == platform.Id);
			}
		}
	}
}
