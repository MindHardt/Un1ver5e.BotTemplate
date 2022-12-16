using Disqord;
using System.Text;

namespace Bot
{
    /// <summary>
    /// Contains various extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Shuffles the collection, making it random-ordered. This is not lazy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The original collection.</param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection, Func<IComparable>? randomizer = null)
        {
            Func<IComparable> random = randomizer ?? (() => Random.Shared.Next());
            return collection.OrderBy((e) => random());
        }

        /// <summary>
        /// Gets random element of a <paramref name="collection"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T GetRandomElement<T>(this IEnumerable<T> collection, Func<IComparable>? randomizer = null)
        {
			Func<IComparable> random = randomizer ?? (() => Random.Shared.Next());
			return collection.MaxBy((e) => random())!;
        }

        /// <summary>
        /// Formats string as a Discord Codeblock
        /// </summary>
        /// <param name="original">The original text</param>
        /// <param name="lang">The language used for formatting, i.e. "CS", "XML", "JSON"...</param>
        /// <returns></returns>
        public static string AsCodeBlock(this string original, string? lang = null) => $"```{lang}\n{original}```";

		/// <summary>
		/// Gets a discord timestamp for <paramref name="time"/>. 
        /// The type of timestamp is defined by <paramref name="type"/>:
        /// <list type="bullet">
        /// <item>R - Relative</item>
        /// <item>d - Short date</item>
        /// <item>D - Long date</item>
        /// <item>t - Short time</item>
        /// <item>T - Long time (including seconds)</item>
        /// <item>f - Short datetime</item>
        /// <item>F - Long datetime</item>
        /// </list>
		/// </summary>
		/// <param name="time"></param>
		/// <param name="type">A letter describing a type of a timestamp.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static string ToDiscordTimestamp(this DateTimeOffset time, char type = 'R')
        {
            char[] types = { 'd', 'D', 't', 'T', 'f', 'F', 'R' };
            if (types.Contains(type) is false) throw new ArgumentOutOfRangeException(nameof(type));
            return $"<t:{time.ToUnixTimeSeconds()}:{type}>";
		}

        /// <summary>
        /// Chunks <paramref name="collection"/> into pages at maximum length of <paramref name="maxPageLen"/>.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="maxPageLen"></param>
        /// <returns></returns>
        public static IList<string> ChunkAtPages(this IEnumerable<string> collection, int maxPageLen)
        {
            Queue<string> strings = new(collection);

            List<string> processed = new();

            StringBuilder sb = new(maxPageLen);
            while (strings.Any())
            {
                string current = strings.Dequeue();

                if (sb.Length + current.Length > maxPageLen)
                {
                    processed.Add(sb.ToString());
                    sb.Clear();
                }
                sb.Append(current);
            }
            processed.Add(sb.ToString());

            return processed;
        }

        /// <summary>
        /// Gets a <see cref="LocalEmbed"/> object which displays data about <paramref name="message"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="guildId">The guild id of the <paramref name="message"/>. Don't leave it null if message was inside a guild or the embed url will be broken.</param>
        /// <returns></returns>
        public static LocalEmbed GetDisplay(this IMessage message, Snowflake? guildId = null)
            => new LocalEmbed()
                .WithDescription(message.Content)
                .WithTimestamp(message.CreatedAt())
                .WithAuthor(message.Author)
                .AddField("Ссылка на сообщение", $"[Ссылка]({Discord.MessageJumpLink(guildId, message.ChannelId, message.Id)})");
    }
}
