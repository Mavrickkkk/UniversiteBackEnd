using UniversiteDomain.Entities;

namespace UniversiteDomain.DataAdapters.DataAdaptersFactory;

public interface IRepositoryFactory
{
    IParcoursRepository ParcoursRepository();
    IEtudiantRepository EtudiantRepository();
    INoteRepository CreateNoteRepository();

    // Méthodes de gestion de la dadasource
    // Ce sont des méthodes qui permettent de gérer l'ensemble du data source
    // comme par exemple tout supprimer ou tout créer
    Task EnsureDeletedAsync();
    Task EnsureCreatedAsync();
    Task SaveChangesAsync();
    Task<object> FindByConditionAsync(Func<object, bool> func);
    Task<Parcours> CreateAsync(Parcours parcours);
    IUeRepository? UeRepository();
}