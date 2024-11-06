using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.ParcoursExceptions;
using UniversiteDomain.Util;

namespace UniversiteDomain.UseCases.ParcoursUseCases.Create;

public class CreateParcoursUseCase(IParcoursRepository ParcoursRepository)
{
    public async Task<Parcours> ExecuteAsync(long ID, string Nom, int Annee)
    {
        var Parcours = new Parcours{Id = ID, NomParcours = Nom, AnneeFormation = Annee};
        return await ExecuteAsync(Parcours);
    }
    public async Task<Parcours> ExecuteAsync(Parcours Parcours)
    {
        await CheckBusinessRules(Parcours);
        Parcours et = await ParcoursRepository.CreateAsync(Parcours);
        ParcoursRepository.SaveChangesAsync().Wait();
        return et;
    }
    private async Task CheckBusinessRules(Parcours parcours)
    {
        ArgumentNullException.ThrowIfNull(parcours);
        ArgumentNullException.ThrowIfNull(parcours.Id);
        ArgumentNullException.ThrowIfNull(parcours.NomParcours);
        ArgumentNullException.ThrowIfNull(parcours.AnneeFormation);
        ArgumentNullException.ThrowIfNull(ParcoursRepository);

        // Vérifie que l'année de formation est 1 ou 2
        if (parcours.AnneeFormation != 1 && parcours.AnneeFormation != 2)
        {
            throw new InvalidAnneeFormationException(parcours.AnneeFormation + " - L'année de formation doit être 1 ou 2.");
        }

        // Vérifie que le nom du parcours n'existe pas déjà
        var parcoursExistant = await ParcoursRepository.FindByConditionAsync(p => p.NomParcours == parcours.NomParcours);
        if (parcoursExistant.Any())
        {
            throw new DuplicateNomParcoursException(parcours.NomParcours + " - Ce parcours existe déjà.");
        }

        // Vérifie que le nom de parcours est suffisamment long (3 caractères minimum)
        if (parcours.NomParcours.Length < 3)
        {
            throw new InvalidNomParcoursException(parcours.NomParcours + " - Le nom du parcours doit contenir au moins 3 caractères.");
        }
    }
    }
    