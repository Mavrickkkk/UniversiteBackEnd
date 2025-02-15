using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteEFDataProvider.Repositories;
using UniversiteEFDataProvider.Data;
using UniversiteDomain.Entities;

namespace UniversiteEFDataProvider.RepositoryFactories;

public class RepositoryFactory(UniversiteDbContext context) : IRepositoryFactory
{
    private IParcoursRepository? _parcours;
    private IEtudiantRepository? _etudiants;
    private IUeRepository? _ues;
    private INoteRepository? _notes;
    
    public IParcoursRepository ParcoursRepository()
    {
        if (_parcours == null)
        {
            _parcours = new ParcoursRepository(context ?? throw new InvalidOperationException());
        }
        return _parcours;
    }

    public IEtudiantRepository EtudiantRepository()
    {
        if (_etudiants == null)
        {
            _etudiants = new EtudiantRepository(context ?? throw new InvalidOperationException());
        }
        return _etudiants;
    }

    public IUeRepository UeRepository()
    {
        if (_ues == null)
        {
            _ues = new UeRepository(context ?? throw new InvalidOperationException());
        }
        return _ues;
    }

    public INoteRepository NoteRepository()
    {
        if (_notes == null)
        {
            _notes = new NoteRepository(context ?? throw new InvalidOperationException());
        }
        return _notes;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task EnsureCreatedAsync()
    {
        await context.Database.EnsureCreatedAsync();
    }

    public async Task EnsureDeletedAsync()
    {
        await context.Database.EnsureDeletedAsync();
    }

    public async Task<Parcours> CreateAsync(Parcours parcours)
    {
        if (parcours == null) throw new ArgumentNullException(nameof(parcours));

        context.Parcours.Add(parcours);
        await context.SaveChangesAsync();
        return parcours;
    }

    public async Task<object> FindByConditionAsync(Func<object, bool> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        // Recherche d'un objet correspondant à la condition
        var result = context.Set<object>().Where(func).FirstOrDefault();
        return result ?? throw new InvalidOperationException("Aucun objet trouvé correspondant à la condition.");
    }
}