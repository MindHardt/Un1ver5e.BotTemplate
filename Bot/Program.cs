﻿using Disqord;
using Disqord.Bot.Hosting;
using Disqord.Gateway;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Bot
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            using IHost host = CreateHost(args);
            host.Run();
        }

		/// <summary>
		/// Creates a <see cref="Microsoft.Extensions.Hosting.IHost"/> object that has all the information about bot.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		internal static IHost CreateHost(string[] args)
        {
            return new HostBuilder()
            .UseSerilog((ctx, logger) =>
            {
                //Configuring logging, it is read from config
                //But we explicitly ignore websocket exception cuz it is noisy but harmless
                logger
                .ReadFrom.Configuration(ctx.Configuration)
                .Filter.ByExcluding(e => e.Exception is Disqord.WebSocket.WebSocketClosedException);
            })
            .ConfigureAppConfiguration(config =>
            {
                config                              //Adding configuration (Microsoft.Extensions.Configuration)
                .AddJsonFile("appsettings.json", true)    //Json file
                .AddCommandLine(args)               //Command line args
                .AddEnvironmentVariables();         //Env variables (used for containers)
            })
            .ConfigureDiscordBot((ctx, bot) =>
            {
                var cfg = ctx.Configuration.GetSection("DiscordBot");  //Reading discord config

                //Reading token, it must be present.
                bot.Token = cfg["Token"] ??
                    throw new ArgumentNullException("Token", "Discord bot token not found, check your configuration.");
            
                //Reading owner ids, they may be absent.
                bot.OwnerIds = cfg.GetSection("OwnerIds").Get<Snowflake[]>();

                //Reading prefixes for text commands. They may be absent.
                bot.Prefixes = cfg.GetSection("Prefixes").Get<string[]>();

                //Bots gateway intents. This should correspond with ones enables in discord developer portal.
                //This may be changed to fit your bot's needs.
                bot.Intents = GatewayIntents.Unprivileged;
            })
            .Build();
        }
    }
}