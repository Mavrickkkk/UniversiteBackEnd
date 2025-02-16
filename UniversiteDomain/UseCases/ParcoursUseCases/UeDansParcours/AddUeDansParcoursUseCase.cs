using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.ParcoursExceptions;
using UniversiteDomain.Exceptions.UeExceptions;

namespace UniversiteDomain.UseCases.ParcoursUseCases.UeDansParcours;

public class AddUeDansParcoursUseCase(IRepositoryFactory repositoryFactory)
{
      public async Task<Parcours> ExecuteAsync(Parcours parcours, Ue ue)
      {
          ArgumentNullException.ThrowIfNull(parcours);
          ArgumentNullException.ThrowIfNull(ue);
          return await ExecuteAsync(parcours.Id, ue.Id); 
      }  
      public async Task<Parcours> ExecuteAsync(long idParcours, long idUe)
      {
          await CheckBusinessRules(idParcours, idUe); 
          return await repositoryFactory.ParcoursRepository().AddUeAsync(idParcours, idUe);
      }

      public async Task<Parcours> ExecuteAsync(Parcours parcours, List<Ue> ues)
      {
          ArgumentNullException.ThrowIfNull(ues);
          ArgumentNullException.ThrowIfNull(parcours);
          long[] idUes = ues.Select(x => x.Id).ToArray();
          return await ExecuteAsync(parcours.Id, idUes); 
      }  
      public async Task<Parcours> ExecuteAsync(long idParcours, long [] idUes)
      { 
        foreach(var id in idUes) await CheckBusinessRules(idParcours, id);
        return await repositoryFactory.ParcoursRepository().AddUeAsync(idParcours, idUes);
      }   

    private async Task CheckBusinessRules(long idParcours, long idUe)
    {
        ArgumentNullException.ThrowIfNull(idParcours);
        ArgumentNullException.ThrowIfNull(idUe);
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idParcours);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idUe);
        
        ArgumentNullException.ThrowIfNull(repositoryFactory);
        ArgumentNullException.ThrowIfNull(repositoryFactory.UeRepository());
        ArgumentNullException.ThrowIfNull(repositoryFactory.UeRepository());
        
        List<Ue> ue = await repositoryFactory.UeRepository().FindByConditionAsync(e=>e.Id.Equals(idUe));
        if (ue == null) throw new UeNotFoundException(idUe.ToString());
        List<Parcours> parcours = await repositoryFactory.ParcoursRepository().FindByConditionAsync(p=>p.Id.Equals(idParcours));
        if (parcours == null) throw new ParcoursNotFoundException(idParcours.ToString());
        
        if (parcours[0].UesEnseignees!=null)
        {
            // Des ues sont déjà enregistrées dans le parcours
            // On recherche si l'ue qu'on veut ajouter n'existe pas déjà
            var trouve= parcours[0].UesEnseignees.FindAll(e=>e.Id.Equals(idUe));
            if (trouve.Count() != 0) throw new DuplicateUeDansParcoursException(idUe+" est déjà présente dans le parcours : "+idParcours);   
        }

    }
    
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}