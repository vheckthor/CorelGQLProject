using CoreGQL.Data;
using CoreGQL.GraphQL.Commands;
using CoreGQL.GraphQL.Platforms;
using CoreGQL.Models;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Subscriptions;
using System.Threading;
using System.Threading.Tasks;

namespace CoreGQL.GraphQL
{
	public class Mutation
	{
		[UseDbContext(typeof(AppDbContext))]
		public async Task<AddPlatformPayload> AddPlatformAsync(
			AddPlatformInput input, 
			[ScopedService] AppDbContext context,
			[Service] ITopicEventSender eventSender,
			CancellationToken cancellationToken)
		{
			var platform = new Platform
			{
				Name = input.Name
			};
			await context.Platform.AddAsync(platform, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
			await eventSender.SendAsync(nameof(Subscription.OnPlatformAdded),platform,cancellationToken);
			return new AddPlatformPayload(platform);
		}

		[UseDbContext(typeof(AppDbContext))]
		public async Task<AddCommandPayload> AddCommandAsync(
			AddCommandInput input, 
			[ScopedService] AppDbContext context,
			[Service] ITopicEventSender eventSender,
			CancellationToken cancellationToken)
		{
			var command = new Command
			{
				HowTo = input.HowTo,
				CommandLine = input.CommandLine,
				PlatformId = input.PlatformId
			};
			await context.Commands.AddAsync(command, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
			await eventSender.SendAsync(nameof(Subscription.OnCommandAdded), command, cancellationToken);
			return new AddCommandPayload(command);
		}
	}
}
