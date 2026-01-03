using System.Diagnostics.CodeAnalysis;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Domain.Common;

public record Result(params Error[] Errors) {

    public virtual bool IsSuccess => Errors.Length == 0;

    public static Result Ok => new();

    public static implicit operator Result(List<Error> errors) => new(errors.ToArray());

    public static implicit operator Result(Error[] errors) => new(errors);

    public static implicit operator Result(Error error) => new(error);

    public static Result operator &(Result r1, Result r2) {
        return r1.IsSuccess && r2.IsSuccess ? Ok : new Result(r1.Errors.Concat(r2.Errors).ToArray());
    }

    public string AllDescriptions => string.Join('\n', Errors.Select(e => e.Description));

    public string? FirstDescription => Errors.FirstOrDefault()?.Description;
    
    public Result<T> WithValueIfOk<T>(T value) where T : class {
        return IsSuccess ? new Result<T>(value) : new Result<T>(null, Errors);
    }

    public Result<T> WithValueIfOk<T>(Func<T> mapper) where T : class {
        return WithValueIfOk(mapper());
    }

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

    public Result Map<TResult>(Func<T, TResult> mapper) where TResult : class {
        return IsSuccess ? new Result<TResult>(mapper(Value!)) : new Result<TResult>(null, Errors);
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

    public static Result<Tuple<T, T>> operator &(Result<T> r1, Result<T> r2) {
        return r1.IsSuccess && r2.IsSuccess
            ? new Tuple<T, T>(r1.Value!, r2.Value!)
            : r1.Errors.Concat(r2.Errors).ToArray();
    }

}
