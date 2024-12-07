using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Svintus.Movies.Application.Models.Results;

namespace Svintus.Microservices.Movies.Extensions;

internal static class ErrorExtensions
{
    public static Status ToRpcStatus(this Error error)
    {
        if (error.Code == ResultCode.ChatIdNotFound)
        {
            return error.ChatIdNotFoundStatus();
        }

        return DefaultStatus();
    }

    private static Status ChatIdNotFoundStatus(this Error error) => new()
    {
        Code = (int)Code.NotFound,
        Message = "Chat id was not found",
        Details =
        {
            Any.Pack(new BadRequest
            {
                FieldViolations =
                {
                    new BadRequest.Types.FieldViolation
                    {
                        Field = "chatId",
                        Description = error.Message
                    }
                }
            })
        }
    };

    private static Status DefaultStatus() => new()
    {
        Code = (int)Code.Internal,
        Message = "Something went wrong"
    };
}