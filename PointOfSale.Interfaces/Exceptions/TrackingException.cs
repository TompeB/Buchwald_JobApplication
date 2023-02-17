namespace PointOfSale.Shared.Exceptions;
public class TrackingException : Exception
{
    public TrackingException(Exception ex)
        : base("The tracking failed because the external service was not reachable or other issues with the interaction happened.",ex)
    { }
}