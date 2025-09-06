namespace SmiTo.Application.Interfaces;

public interface IShortCodeGenerator
{
    string GenerateShortCode(int length = 6);
    Task<string> GenerateUniqueShortCodeAsync(int length = 6);
}
