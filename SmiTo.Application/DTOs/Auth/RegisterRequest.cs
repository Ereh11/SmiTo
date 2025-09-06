namespace SmiTo.Application.DTOs.Auth;

public record RegisterRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password
);
