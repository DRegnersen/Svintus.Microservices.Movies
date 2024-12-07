namespace Svintus.Movies.Application.Models.Results;

public readonly struct Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;

    public Result(TValue value)
    {
        _value = value;
    }

    public Result(TError error)
    {
        _error = error;
    }

    public bool IsSuccess => _value is not null;

    public bool IsFail => _error is not null;

    public TValue Value => _value ?? throw new InvalidOperationException("Result has been failed");

    public TError Error => _error ?? throw new InvalidOperationException("Result has been succeeded");

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);

    public static implicit operator Result<TValue, TError>(TError error) => new(error);
}