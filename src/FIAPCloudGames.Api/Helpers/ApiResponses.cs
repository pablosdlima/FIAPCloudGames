namespace FIAPCloudGames.Api.Helpers;

public static class ApiResponses
{
    public static IResult Ok(object data, string message = "Operação realizada com sucesso.")
    {
        return Results.Ok(new
        {
            statusCode = 200,
            message,
            data
        });
    }

    public static IResult OkMessage(string message)
    {
        return Results.Ok(new
        {
            statusCode = 200,
            message
        });
    }

    public static IResult Created(string location, object data, string message = "Recurso criado com sucesso.")
    {
        return Results.Created(location, new
        {
            statusCode = 201,
            message,
            data
        });
    }

    public static IResult NotFound(string key, string message)
    {
        return Results.NotFound(new
        {
            statusCode = 404,
            message = "Validation failed",
            errors = new Dictionary<string, string[]>
            {
                { key, new[] { message } }
            }
        });
    }

    public static IResult BadRequest(string key, string message)
    {
        return Results.BadRequest(new
        {
            statusCode = 400,
            message = "Validation failed",
            errors = new Dictionary<string, string[]>
            {
                { key, new[] { message } }
            }
        });
    }

    public static IResult BadRequestMultiple(Dictionary<string, string[]> errors)
    {
        return Results.BadRequest(new
        {
            statusCode = 400,
            message = "Validation failed",
            errors
        });
    }

    public static IResult Unauthorized(string key, string message)
    {
        return Results.Json(new
        {
            statusCode = 401,
            message = "Authentication failed",
            errors = new Dictionary<string, string[]>
            {
                { key, new[] { message } }
            }
        }, statusCode: 401);
    }

    public static IResult Problem(string detail = "Erro interno no servidor.")
    {
        return Results.Problem(
            detail: detail,
            statusCode: 500
        );
    }

    public static IResult BadRequestMultiple(string message, Dictionary<string, string[]> errors)
    {
        var response = new
        {
            statusCode = StatusCodes.Status400BadRequest,
            message,
            errors
        };
        return Results.BadRequest(response);
    }
}