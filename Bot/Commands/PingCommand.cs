using Disqord;
using Disqord.Bot.Commands.Application;
using Qmmands;
using System.Diagnostics;
using Bot.Commands.DeleteThisButton;

namespace Bot.Commands
{
    public class PingCommand : DiscordApplicationGuildModuleBase
    {
        //PING
        [SlashCommand("пинг")]
        [Description("Проверка задержки ответа бота.")]
        public IResult Ping()
        {
            TimeSpan latency = DateTimeOffset.Now - Context.Interaction.CreatedAt();
            DateTimeOffset launchTime = Process.GetCurrentProcess().StartTime;

            string launchTimeStamp = launchTime.ToRelativeDiscordTime();
            string latencyTimeStamp = $"Задержка сокета `{((int)latency.TotalMilliseconds)}`мс.";

            LocalInteractionMessageResponse response = new LocalInteractionMessageResponse()
                .AddEmbed(new LocalEmbed()
                    .WithTitle(latencyTimeStamp)
                    .AddField(new LocalEmbedField()
                        .WithName("Бот запущен")
                        .WithValue(launchTimeStamp)))
                .AddDeleteThisButton();

            return Response(response);
        }
    }
}
