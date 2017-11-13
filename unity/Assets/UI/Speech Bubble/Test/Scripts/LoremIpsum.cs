using rharel.Debug;
using System.Linq;

/// <summary>
/// This utility class produces dummy-text for testing purposes. See
/// <a href="http://www.lipsum.com/">lipsum.com</a> for more information.  
/// </summary>
public static class LoremIpsum
{
    /// <summary>
    /// Produces a text with the specified number of words.
    /// </summary>
    /// <param name="count">The desired number of words.</param>
    /// <returns>A string with the specified number of words.</returns>
    /// <remarks>
    /// <paramref name="count"/> must be at least 0.
    /// </remarks>
    public static string Words(int count)
    {
        Require.IsAtLeast(count, 0);

        return GetTokenString(_words, count, delimeter: " ");
    }
    /// <summary>
    /// Produces a text with the specified number of sentences.
    /// </summary>
    /// <param name="count">The desired number of sentences.</param>
    /// <returns>A string with the specified number of sentences.</returns>
    /// <remarks>
    /// <paramref name="count"/> must be at least 0.
    /// </remarks>
    public static string Sentences(int count)
    {
        Require.IsAtLeast(count, 0);

        return GetTokenString(_sentences, count, delimeter: ". ") + '.';
    }

    /// <summary>
    /// Produces a text with the specified number of tokens from the specified
    /// collection.
    /// </summary>
    /// <param name="tokens">The token collection to sample from.</param>
    /// <param name="count">The desired number of tokens.</param>
    /// <param name="delimeter">The desired token delimiter.</param>
    /// <returns>A string with the specified number of tokens.</returns>
    /// <remarks>
    /// <paramref name="count"/> must be at least 0.
    /// </remarks>
    private static string GetTokenString
    (
        string[] tokens, 
        int count,
        string delimeter
    )
    {
        if (tokens.Length == 0 || count == 0) { return string.Empty; }

        string[] tokens_in_range = new string[count];
        for (int i = 0; i < count; ++i)
        {
            tokens_in_range[i] = tokens[i % tokens.Length];
        }

        return string.Join(delimeter, tokens_in_range);
    }

    /// <summary>
    /// The original passage.
    /// </summary>
    private static readonly string _text =
@"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod 
tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, 
quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo 
consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse 
cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non 
proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

    private static readonly string[] _words = (
        _text.Split(' ')
             .Select(word => word.Trim())
             .ToArray()
    );
    private static readonly string[] _sentences = (
        _text.Replace("\n", string.Empty)
             .Split('.')
             .Select(sentence => sentence.Trim())
             .ToArray()
    );
}
