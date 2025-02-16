using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;
using UniversiteDomain.Exceptions.NotesException;
using UniversiteDomain.Exceptions.ParcoursExceptions;
using UniversiteDomain.Exceptions.UeExceptions;

namespace UniversiteDomain.UseCases.EtudiantUseCases.NotesDansEtudiant;

public class AddNotesDansEtudiantUseCase(IRepositoryFactory repositoryFactory)
{
    
    public async Task<Etudiant> ExecuteAsync(long idEtudiant, long idNote)
    {
        await CheckBusinessRules(idEtudiant, idNote); 
        return await repositoryFactory.EtudiantRepository().AddNoteAsync(idEtudiant, idNote);
    }

    public async Task<Etudiant> ExecuteAsync(long idetudiant, long [] idNotes)
    {
        foreach (var id in idNotes) await CheckBusinessRules(idetudiant, id);
        return await repositoryFactory.EtudiantRepository().AddNoteAsync(idetudiant, idNotes);
    }   

    private async Task CheckBusinessRules(long idEtudiant, long idNote)
    {
        ArgumentNullException.ThrowIfNull(idEtudiant);
        ArgumentNullException.ThrowIfNull(idNote);
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idEtudiant);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idNote);
        
        ArgumentNullException.ThrowIfNull(repositoryFactory);
        ArgumentNullException.ThrowIfNull(repositoryFactory.EtudiantRepository());
        ArgumentNullException.ThrowIfNull(repositoryFactory.UeRepository());
        ArgumentNullException.ThrowIfNull(repositoryFactory.NotesRepository());
        ArgumentNullException.ThrowIfNull(repositoryFactory.ParcoursRepository());
        
        List<Etudiant> etudiant = await repositoryFactory.EtudiantRepository().FindByConditionAsync(e=>e.Id.Equals(idEtudiant));
        if (etudiant is { Count: 0 } || etudiant is null) throw new EtudiantNotFoundException(idEtudiant.ToString());

        List<Notes> note = await repositoryFactory.NotesRepository().FindByConditionAsync(e=>e.Id.Equals(idNote));
        if (note is { Count: 0 } || note is null) throw new NotesNotFoundException(idNote.ToString());
        
        List<Ue> ue = await repositoryFactory.UeRepository().FindByConditionAsync(e => e.Id.Equals(note[0].UeId));
        if (ue is { Count: 0 } || ue is null) throw new UeNotFoundException(note[0].UeId.ToString());
        
        List<Parcours> parcours = await repositoryFactory.ParcoursRepository()
            .FindByConditionAsync(p => p.Inscrits.Find(inscri => inscri.Id.Equals(idEtudiant)) != null && p.UesEnseignees.Find(ens => ens.Id.Equals(ue[0].Id)) != null);
        if (parcours is { Count: 0 } || ue is null) throw new ParcoursNotFoundException(etudiant[0].ParcoursSuivi.Id.ToString());
        
        
        // Vérifie si la note existe déjà dans la liste de l'etudiant
        if (etudiant[0].NotesObtenues != null)
        {
            var trouve= etudiant[0].NotesObtenues.FindAll(e=>e.UeId.Equals(note[0].UeId));
            if (trouve.Count() != 0) throw new DuplicateNoteException(note[0].ToString() + "Existe déjà dans la liste de l'etudiant");
        }
    }
}