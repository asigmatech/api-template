using System.Linq.Expressions;
using AsigmaApiTemplate.Api.Models;
using AsigmaApiTemplate.Api.SearchObjects;

namespace AsigmaApiTemplate.Api.Helpers;

public static class PredicateBuilder
{
    public static Expression<Func<WeatherForecast, bool>> BuildWeatherForecastPredicate(SearchWeatherForecast request)
    {
        Expression<Func<WeatherForecast, bool>> predicate = q => true;

        if (request.Id.HasValue)
        {
            predicate = predicate.AndAlso(q => q.Id == request.Id.Value);
        }

        if (request.Date.HasValue)
        {
            predicate = predicate.AndAlso(q => q.Date == request.Date.Value);
        }

        if (request.TemperatureC.HasValue)
        {
            predicate = predicate.AndAlso(q => q.TemperatureC == request.TemperatureC.Value);
        }

        if (request.Ids.Count != 0)
        {
            predicate = predicate.AndAlso(q => request.Ids.Contains(q.Id));
        }

        return predicate;
    }

    private static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));

        var body = Expression.AndAlso(
            Expression.Invoke(expr1, parameter),
            Expression.Invoke(expr2, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}