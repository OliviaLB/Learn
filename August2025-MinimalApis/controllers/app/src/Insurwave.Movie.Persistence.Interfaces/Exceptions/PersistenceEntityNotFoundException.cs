namespace Insurwave.Movie.Persistence.Interfaces.Exceptions;

public class PersistenceEntityNotFoundException : Exception
{
    private const string MovieMessageFormat = "Movie with id '{0}' for owning organisation '{1}' was not found";
    private const string DefaultNotFound = "Entity with id '{0}' was not found";

    public PersistenceEntityNotFoundException(string schema, Guid id, Guid owningOrganisationId)
    {
        Id = id;
        OwningOrganisationId = owningOrganisationId;
        MessageFormat = schema switch
        {
            nameof(Contracts.Movie) => MovieMessageFormat,
            _ => DefaultNotFound
        };
    }

    private string MessageFormat { get; }
    private Guid Id { get; }
    private Guid OwningOrganisationId { get; }

    public override string Message => string.Format(MessageFormat, Id, OwningOrganisationId);
}
