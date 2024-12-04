namespace UniversiteDomain.Exceptions.ParcoursExceptions;

public class DuplicateNomParcoursException : Exception
{
    public DuplicateNomParcoursException() : base() { }
    public DuplicateNomParcoursException(string message) : base(message) { }
    public DuplicateNomParcoursException(string message, Exception inner) : base(message, inner) { }
}