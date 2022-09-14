using Disqord;
using Disqord.Bot.Hosting;
using Disqord.Gateway;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Bot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var host = CreateHost(args); //Creating host
            host.Run(); //Running
        }

        /// <summary>
        /// This creates a <see cref="Microsoft.Extensions.Hosting.IHost"/> object that has all the information about bot.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static IHost CreateHost(string[] args)
        {
            return new HostBuilder()
            .UseSerilog((ctx, logger) =>
            {
                logger                                          //Configuring logging
                .ReadFrom.Configuration(ctx.Configuration);     //It is read from config
            })
            .ConfigureAppConfiguration(config =>
            {
                config                              //Adding configuration (Microsoft.Extensions.Configuration)
                .AddJsonFile("appsettings.json")    //Json file
                .AddCommandLine(args)               //Command line args
                .AddEnvironmentVariables();         //Env variables (used for containers)
            })
            .ConfigureDiscordBot((ctx, bot) =>
            {
                IConfigurationSection config = ctx.Configuration.GetSection("DiscordBot");  //Reading discord config

                //Reading token, it must be present.
                bot.Token = config["Token"] ??
                    throw new ArgumentNullException("Token", "Token not found, check your configuration.");

                //Reading owner ids, they may be absent.
                bot.OwnerIds = config.GetSection("OwnerIds").Get<ulong[]>()?.Select(id => (Snowflake)id);

                //Reading prefixes for text commands. They may be absent.
                bot.Prefixes = config.GetSection("Prefixes").Get<string[]>();

                //Bots gateway intents. This should correspond with ones enables in discord developer portal.
                //This may be changed to fit your bot's needs.
                bot.Intents = GatewayIntents.Unprivileged;
            })
            .Build();
        }
    }
}