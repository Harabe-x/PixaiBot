using System.Text;

namespace PixaiBot.Business_Logic.Extension;

internal static class StringBuilderExtensions
{
    /// <summary>
    ///     Appends a line to the end of the <see cref="StringBuilder" /> if the condition is true.
    /// </summary>
    /// <param name="stringBuilder"></param>
    /// <param name="condition"></param>
    /// <param name="value"></param>
    public static void AppendLineIf(this StringBuilder stringBuilder, bool condition, string value)
    {
        if (condition) stringBuilder.AppendLine(value);
    }
}