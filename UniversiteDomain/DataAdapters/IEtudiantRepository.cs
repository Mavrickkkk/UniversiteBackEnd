using UniversiteDomain.Entities;

namespace UniversiteDomain.DataAdapters;

public interface IEtudiantRepository : IRepository<Etudiant>
{
    Task<Etudiant?> GetByNumEtudAsync(string numEtud);
    Task<IEnumerable<Etudiant>> GetByParcoursAsync(long idParcours);
    
    Task<Etudiant> AddNoteAsync(long idEtudiant, long idNote);
    Task<Etudiant> AddNoteAsync(Etudiant etudiant, Note note);
    Task<Etudiant> AddNoteAsync(Etudiant? etudiant, List<Note> notes);
    Task<Etudiant> AddNoteAsync(long idEtudiant, long[] idNotes);

    Task<Etudiant> AddUeAsync(long idEtudiant, long idUe);
    Task<Etudiant> AddUeAsync(Etudiant etudiant, Ue ue);
    Task<Etudiant> AddUeAsync(Etudiant? etudiant, List<Ue> ues);
    Task<Etudiant> AddUeAsync(long idEtudiant, long[] idUes);
    Task AffecterParcoursAsync(long idEtudiant, long idParcours);
    Task AffecterParcoursAsync(Etudiant etudiant, Parcours parcours);
}