using Disqord;
using Disqord.Bot.Commands.Components;
using Disqord.Rest;
using Qmmands;

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
        public async ValueTask<IResult> DeleteMessageButtonCommand(Snowflake restrictedToId)
        {
            if (Context.AuthorId == restrictedToId)
            {
                IMessage msg = (Context.Interaction as IComponentInteraction)!.Message;

                await Bot.DeleteMessageAsync(msg.ChannelId, msg.Id);
                return Results.Success;
            }
            return Results.Failure("Кнопки самоуничтожения синего цвета доступны только тому, кто вызвал сообщение, красные доступны всем.");
        }

        //This fires when a public button is pressed
        [ButtonCommand(DeleteThisButtonId)]
        public async ValueTask DeleteMessageButtonCommand()
        {
            IMessage msg = (Context.Interaction as IComponentInteraction)!.Message;

            await Bot.DeleteMessageAsync(msg.ChannelId, msg.Id);
        }
    }
}
