using Disqord;
using Disqord.Bot.Commands.Components;
using Disqord.Rest;

namespace Bot.Commands.DeleteThisButton
{
	public class DeleteThisButtonComponentCommand : DiscordComponentModuleBase
	{
		/// <summary>
		/// This is a custom ID for all "delete me" buttons. This is recommended to make this unique per bot.
		/// </summary>
		public const string DeleteThisButtonId = "delete_this";

		//This fires when a button restricted to author is pressed
		[ButtonCommand(DeleteThisButtonId + ":*")]
		public async ValueTask DeleteMessageButtonCommand(Snowflake restrictedToId)
		{
			if (Context.AuthorId == restrictedToId)
				await DeleteMessage();
			else
				await Context.Interaction.Response().SendMessageAsync(new LocalInteractionMessageResponse()
					.WithIsEphemeral()
					.WithContent("У вас нет права удалять это сообщение."));
		}

		//This fires when a public button is pressed
		[ButtonCommand(DeleteThisButtonId)]
		public ValueTask DeleteMessageButtonCommand() => DeleteMessage();

		private async ValueTask DeleteMessage()
		{
			IMessage msg = (Context.Interaction as IComponentInteraction)!.Message;

			await Bot.DeleteMessageAsync(msg.ChannelId, msg.Id);
		}
	}
}
