using PointOfSale.Interfaces;

namespace PointOfSale.Shared.Exceptions;
public class ValidationException : Exception
{
    public ValidationException(ISale name)
    : base($"The sales event was missing one or more required values. Article Number '{name?.ArticleNumber}', SalesPrice '{name?.SalesPrice}'")
    {

    }
}