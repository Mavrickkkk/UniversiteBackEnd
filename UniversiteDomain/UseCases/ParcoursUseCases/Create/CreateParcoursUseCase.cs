using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.ParcoursExceptions;
using UniversiteDomain.Util;

namespace UniversiteDomain.UseCases.ParcoursUseCases.Create;

public class CreateParcoursUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<Parcours> ExecuteAsync(long ID, string Nom, int Annee)
    {
        var Parcours = new Parcours{Id = ID, NomParcours = Nom, AnneeFormation = Annee};
        return await ExecuteAsync(Parcours);
    }
    public async Task<Parcours> ExecuteAsync(Parcours Parcours)
    {
        await CheckBusinessRules(Parcours);
        Parcours et = await repositoryFactory.ParcoursRepository().CreateAsync(Parcours);
        repositoryFactory.ParcoursRepository().SaveChangesAsync().Wait();
        return et;
    }
    
    private async Task CheckBusinessRules(Parcours parcours)
    {
        ArgumentNullException.ThrowIfNull(parcours);
        ArgumentNullException.ThrowIfNull(parcours.Id);
        ArgumentNullException.ThrowIfNull(parcours.NomParcours);
        ArgumentNullException.ThrowIfNull(repositoryFactory.ParcoursRepository());
        
        // On recherche un parcours avec le même nom
        List<Parcours> existe = await repositoryFactory.ParcoursRepository().FindByConditionAsync(e=>e.NomParcours.Equals(parcours.NomParcours));

        // Si un parcours avec le même nom existe déjà, on lève une exception personnalisée
        if (existe .Any()) throw new DuplicateNomParcoursException(parcours.NomParcours+ " - ce parcours est déjà existe déjà");
        
        // Vérification du format de l'année de formation
        if (parcours.AnneeFormation != 1 && parcours.AnneeFormation != 2) throw new InvalidAnneeFormationException(parcours.NomParcours + " - Année de parcours incorrecte");
    }
}
    