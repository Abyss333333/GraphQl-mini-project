using CommanderGQL.Data;
using CommanderGQL.GraphQL.Commands;
using CommanderGQL.GraphQL.Platforms;
using CommanderGQL.Models;
using HotChocolate.Subscriptions;

namespace CommanderGQL.GraphQL
{
    public class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        public async Task<PlatformPayload> AddPlatformAsync(AddPlatformInput input, [ScopedService] AppDbContext context,
            [Service] ITopicEventSender eventSender, CancellationToken cancellationToken)
        {
            var platform = new Platform
            {
                Name = input.Name,
                LicenseKey = Guid.NewGuid().ToString()
            };

            context.Platforms.Add(platform);
            await context.SaveChangesAsync(cancellationToken);

            await eventSender.SendAsync(nameof(Subscription.OnPlatformAdded), platform, cancellationToken);

            return new PlatformPayload(platform);
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<CommandPayload> AddCommandAsync(AddCommandInput input, [ScopedService] AppDbContext context)
        {
            var command = new Command
            {
                HowTo = input.HowTo,
                PlatformId = input.PlatformId,
                CommandLine = input.CommandLine
            };

            context.Commands.Add(command);
            await context.SaveChangesAsync();
            return new CommandPayload(command);
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<CommandPayload> UpdateCommandAsync(UpdateCommandInput input, [ScopedService] AppDbContext context)
        {
            var command = context.Commands.Where(e => e.Id == input.id).FirstOrDefault();

            if (command == null)
                return null;

            command.HowTo = input.HowTo != null ? input.HowTo : command.HowTo;
            command.PlatformId = input.PlatformId != null ? (int)input.PlatformId : command.PlatformId;
            command.CommandLine = input.CommandLine != null ? input.CommandLine : command.CommandLine;

            await context.SaveChangesAsync();
            return new CommandPayload(command);
        }
    }
}