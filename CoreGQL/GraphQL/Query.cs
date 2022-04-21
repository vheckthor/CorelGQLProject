using CoreGQL.Data;
using CoreGQL.Models;
using HotChocolate;
using HotChocolate.Data;
using System.Linq;

namespace CoreGQL.GraphQL
{
	public class Query
	{
		[UseDbContext(typeof(AppDbContext))]
		[UseFiltering]
		[UseSorting]
		public IQueryable<Platform> GetPlatform([ScopedService] AppDbContext context)
		{
			return context.Platform;
		}

		[UseDbContext(typeof(AppDbContext))]
		[UseFiltering]
		[UseSorting]
		public IQueryable<Command> GetCommand([ScopedService] AppDbContext context)
		{
			return context.Commands;
		}
	}
}
