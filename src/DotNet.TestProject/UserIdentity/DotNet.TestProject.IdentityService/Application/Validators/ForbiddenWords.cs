namespace DotNet.TestProject.IdentityService.Application.Validators;

/// <summary>
///  Helper class for checking is there are Forbidden Words
///  which are not allowed to be used during registration
/// </summary>
public static class ForbiddenWords
{
    /// <summary>
    /// Check Is there are forbidden word.
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public static string ForbiddenWordChecker(string word)
    {
        if(string.IsNullOrEmpty(word))
            return string.Empty;

        List<string> forbiddenWords = new()
        { "admin", "administrator", "username", "password", "name", "firstname", "lastname", "email", "e-mail", "string", "number", "boolean" };

        foreach (string forbidden in forbiddenWords)
        {
            if (word.ToLower().Contains(forbidden.ToLower()))
                return forbidden;
        }

        return string.Empty;
    }
}