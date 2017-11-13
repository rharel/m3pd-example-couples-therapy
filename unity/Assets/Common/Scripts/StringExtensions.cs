using rharel.Debug;
/// <summary>
/// A collection of extension methods for strings.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Replaces the character at the specified index with another.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <param name="index">The index of the character to replace.</param>
    /// <param name="new_character">The new character.</param>
    /// <returns>
    /// A new string with the character at the specified position 
    /// replaced by the new one.
    /// </returns>
    public static string ReplaceAt(
        this string input, 
        int index, 
        char new_character)
    {
        Require.IsNotNull(input);

        char[] characters = input.ToCharArray();
        characters[index] = new_character;

        return new string(characters);
    }
}
