public class GeneralResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();

    public static GeneralResult Failure(List<string> errors, string message = "Request failed")
        => new GeneralResult { Success = false, Errors = errors, Message = message };
}

public class GeneralResult<T> : GeneralResult
{
    public T? Data { get; set; }

    public static GeneralResult<T> SuccessResult(T data, string message = "Request successful")
        => new GeneralResult<T> { Success = true, Data = data, Message = message };

    public new static GeneralResult<T> Failure(List<string> errors, string message = "Request failed")
        => new GeneralResult<T> { Success = false, Errors = errors, Message = message };
}
