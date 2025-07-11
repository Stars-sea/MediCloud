using System.Diagnostics.CodeAnalysis;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Common.Contracts;

public record Result(params Error[] Errors) {

    public virtual bool IsSuccess => Errors.Length == 0;
    
    public static Result Ok => new();

    public static implicit operator Result(List<Error> errors) => new(errors.ToArray());

    public static implicit operator Result(Error[] errors) => new(errors);
    
    public static implicit operator Result(Error error) => new(error);
    
    public virtual TResult Match<TResult>(Func<TResult> onSuccess, Func<Error[], TResult> onError) {
        return IsSuccess ? onSuccess() : onError(Errors);
    }

    public virtual TResult MatchFirst<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onError) {
        return IsSuccess ? onSuccess() : onError(Errors.First());
    }
}

public record Result<T>(T? Value, params Error[] Errors) : Result(Errors) where T : class {
    
    /// <summary>
    /// <para>Convert base type to current type. Kept for reflection</para>
    /// <see cref="MediCloud.Application.Common.Validators.ValidationConsumeFilter{T}"/>
    /// </summary>
    public Result(Result result) : this(null, result.Errors) { }
    
    public bool TryGet([MaybeNullWhen(false)] out T result) {
        if (!IsSuccess) {
            result = Value!;
            return true;
        }

        result = null;
        return false;
    }

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error[], TResult> onError) {
        return IsSuccess ? onSuccess(Value!) : onError(Errors);
    }

    public TResult MatchFirst<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onError) {
        return IsSuccess ? onSuccess(Value!) : onError(Errors.First());
    }

    public static implicit operator Result<T>(T value) => new(value);

    public static implicit operator Result<T>(Error error) => new(null, error);

    public static implicit operator Result<T>(List<Error> errors) => new(null, errors.ToArray());

    public static implicit operator Result<T>(Error[] errors) => new(null, errors.ToArray());

}
