using System;
using StudentGradingSystem.Core.Security;
using Xunit;

namespace StudentGradingSystem.Core.Tests.Unit;

public class PasswordHasherTests
{
    // Copied from database/02-seed.sql (user "jsmith"); generated independently with Python's hashlib.
    private const string SeedHashForStudentPassword =
        "pbkdf2-sha256$210000$wtzAD7yhlBzSGABwLMH3Lw==$7ZbPJEJ+tscL01i7BEgPhY7dGk+LPDs+gOk54U4MUag=";

    [Fact]
    public void Hash_ThenVerify_WithSamePassword_Succeeds()
    {
        string hash = PasswordHasher.Hash("correct horse battery staple", PasswordHasher.MinimumIterations);

        Assert.True(PasswordHasher.Verify("correct horse battery staple", hash));
    }

    [Fact]
    public void Verify_WithWrongPassword_Fails()
    {
        string hash = PasswordHasher.Hash("Secret#1", PasswordHasher.MinimumIterations);

        Assert.False(PasswordHasher.Verify("secret#1", hash));
        Assert.False(PasswordHasher.Verify(string.Empty, hash));
    }

    [Fact]
    public void Hash_UsesRandomSalt_SoEqualPasswordsProduceDifferentHashes()
    {
        string first = PasswordHasher.Hash("same", PasswordHasher.MinimumIterations);
        string second = PasswordHasher.Hash("same", PasswordHasher.MinimumIterations);

        Assert.NotEqual(first, second);
    }

    [Fact]
    public void Hash_WritesSelfDescribingFormat()
    {
        string hash = PasswordHasher.Hash("x", 12_345);
        string[] parts = hash.Split('$');

        Assert.Equal(4, parts.Length);
        Assert.Equal(PasswordHasher.Scheme, parts[0]);
        Assert.Equal("12345", parts[1]);
        Assert.Equal(16, Convert.FromBase64String(parts[2]).Length);
        Assert.Equal(32, Convert.FromBase64String(parts[3]).Length);
    }

    [Fact]
    public void Verify_AcceptsHashesFromTheSeedScript()
    {
        Assert.True(PasswordHasher.Verify("Student@123", SeedHashForStudentPassword));
        Assert.False(PasswordHasher.Verify("Student@1234", SeedHashForStudentPassword));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("1234")]
    [InlineData("pbkdf2-sha256$210000$onlythreeparts")]
    [InlineData("md5$210000$c2FsdA==$aGFzaA==")]
    [InlineData("pbkdf2-sha256$abc$c2FsdA==$aGFzaA==")]
    [InlineData("pbkdf2-sha256$-5$c2FsdA==$aGFzaA==")]
    [InlineData("pbkdf2-sha256$100$c2FsdA==$aGFzaA==")]
    [InlineData("pbkdf2-sha256$210000$not base64!$aGFzaA==")]
    [InlineData("pbkdf2-sha256$210000$$aGFzaA==")]
    public void Verify_ReturnsFalse_ForMalformedHashes(string? storedHash)
    {
        Assert.False(PasswordHasher.Verify("anything", storedHash));
    }

    [Fact]
    public void Hash_RejectsWeakWorkFactor()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => PasswordHasher.Hash("x", 1_000));
    }

    [Fact]
    public void Hash_RejectsNullPassword()
    {
        Assert.Throws<ArgumentNullException>(() => PasswordHasher.Hash(null!));
    }
}
