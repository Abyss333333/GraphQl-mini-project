namespace CommanderGQL.GraphQL.Commands
{
    public record AddCommandInput(string HowTo, string CommandLine, int PlatformId);

    public record UpdateCommandInput(int id, string? HowTo, string? CommandLine, int? PlatformId);

}