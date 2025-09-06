using SmiTo.Application.Interfaces;
using SmiTo.Domain.Repositories;

namespace SmiTo.Application.Services;

public class ShortCodeGenerator : IShortCodeGenerator
{
    private readonly IUnitOfWork _unitOfWork;
    private static readonly char[] Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
    private static readonly Random Random = new();

    public ShortCodeGenerator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private string GenerateShortCode(int length)
    {
        var chars = new char[length];
        for (int i = 0; i < length; i++)
            chars[i] = Characters[Random.Next(Characters.Length)];
        return new string(chars);
    }

    public async Task<string> GenerateUniqueShortCodeAsync(int length = 6)
    {
        var maxAttempts = 10;
        for (var attempt = 0; attempt < maxAttempts; attempt++)
        {
            var shortCode = GenerateShortCode(length);

            if (!await _unitOfWork.URLRepository.ShortCodeExistsAsync(shortCode))
                return shortCode;

            if (attempt == 5) length++;
        }

        throw new InvalidOperationException("Could not generate unique short code");
    }

    string IShortCodeGenerator.GenerateShortCode(int length)
    {
        return GenerateShortCode(length);
    }
}
