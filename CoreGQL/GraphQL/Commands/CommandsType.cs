using CoreGQL.Data;
using CoreGQL.Models;
using HotChocolate;
using HotChocolate.Types;
using System.Linq;

namespace CoreGQL.GraphQL.Commands
{
	public class CommandsType : ObjectType<Command>
	{
		protected override void Configure(IObjectTypeDescriptor<Command> descriptor)
		{
			descriptor.Description("Represents any executable command");
			descriptor
				.Field(_ => _.Platform)
				.ResolveWith<Resolvers>(x=>x.GetPlatform(default!, default!))
				.UseDbContext<AppDbContext>()
				.Description("This is the platform to which the command belong to");
		}

		private class Resolvers
		{
			public Platform GetPlatform(Command command, [ScopedService] AppDbContext context)
			{
				return context.Platform.FirstOrDefault(_ => _.Id == command.PlatformId);
			}
		}
	}
}
