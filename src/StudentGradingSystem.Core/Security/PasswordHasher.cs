using System;
using System.Globalization;
using System.Security.Cryptography;

namespace StudentGradingSystem.Core.Security;

/// <summary>
/// Salted PBKDF2 (HMAC-SHA256) password hashing.
/// </summary>
/// <remarks>
/// Hashes are stored as a self-describing string so the work factor can be raised
/// later without breaking existing accounts:
/// <c>pbkdf2-sha256$&lt;iterations&gt;$&lt;base64 salt&gt;$&lt;base64 hash&gt;</c>.
/// </remarks>
public static class PasswordHasher
{
    /// <summary>Identifier written at the start of every hash.</summary>
    public const string Scheme = "pbkdf2-sha256";

    /// <summary>Work factor used for new hashes.</summary>
    public const int DefaultIterations = 210_000;

    internal const int MinimumIterations = 10_000;
    private const int SaltSizeBytes = 16;
    private const int HashSizeBytes = 32;

    /// <summary>Creates a new salted hash for <paramref name="password"/>.</summary>
    public static string Hash(string password, int iterations = DefaultIterations)
    {
        if (password is null)
        {
            throw new ArgumentNullException(nameof(password));
        }

        if (iterations < MinimumIterations)
        {
            throw new ArgumentOutOfRangeException(nameof(iterations), $"Use at least {MinimumIterations:N0} iterations.");
        }

        byte[] salt = new byte[SaltSizeBytes];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        byte[] hash = Derive(password, salt, iterations, HashSizeBytes);

        return string.Join(
            "$",
            Scheme,
            iterations.ToString(CultureInfo.InvariantCulture),
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash));
    }

    /// <summary>
    /// Checks <paramref name="password"/> against a hash produced by <see cref="Hash"/>.
    /// Returns <c>false</c> (never throws) for malformed or unknown hash formats.
    /// </summary>
    public static bool Verify(string password, string? storedHash)
    {
        if (password is null || string.IsNullOrWhiteSpace(storedHash))
        {
            return false;
        }

        string[] parts = storedHash!.Split('$');
        if (parts.Length != 4 || !string.Equals(parts[0], Scheme, StringComparison.Ordinal))
        {
            return false;
        }

        if (!int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out int iterations)
            || iterations < MinimumIterations)
        {
            return false;
        }

        byte[] salt;
        byte[] expected;
        try
        {
            salt = Convert.FromBase64String(parts[2]);
            expected = Convert.FromBase64String(parts[3]);
        }
        catch (FormatException)
        {
            return false;
        }

        if (salt.Length == 0 || expected.Length == 0)
        {
            return false;
        }

        byte[] actual = Derive(password, salt, iterations, expected.Length);
        return FixedTimeEquals(actual, expected);
    }

    private static byte[] Derive(string password, byte[] salt, int iterations, int length)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(length);
    }

    /// <summary>Constant-time comparison so timing doesn't leak how many bytes matched.</summary>
    private static bool FixedTimeEquals(byte[] left, byte[] right)
    {
        if (left.Length != right.Length)
        {
            return false;
        }

        int difference = 0;
        for (int i = 0; i < left.Length; i++)
        {
            difference |= left[i] ^ right[i];
        }

        return difference == 0;
    }
}
