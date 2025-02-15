using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using System.Threading.Tasks;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;

namespace UniversiteDomain.UseCases.NoteUseCases.Create
{
    public class CreateNoteUseCase
    {
        private readonly INoteRepository _noteRepository;

        // Modifie le constructeur pour accepter IRepositoryFactory
        public CreateNoteUseCase(IRepositoryFactory repositoryFactory)
        {
            // Utilise le factory pour obtenir le INoteRepository
            _noteRepository = repositoryFactory.CreateNoteRepository();
        }

        public async Task<Note> ExecuteAsync(Etudiant etudiant, Ue ue, double note)
        {
            // Vérification si l'étudiant est inscrit à l'UE
            if (!etudiant.Ues.Contains(ue))
                throw new InvalidOperationException("L'étudiant n'est pas inscrit dans cette UE.");

            // Vérification de la validité de la note
            if (note < 0 || note > 20)
                throw new ArgumentException("La note doit être comprise entre 0 et 20.");

            // Vérification qu'il n'existe pas déjà une note pour cet étudiant dans cette UE
            var existingNote = await _noteRepository.FindNoteByEtudiantUeAsync(etudiant.Id, ue.Id);
            if (existingNote != null)
                throw new InvalidOperationException("L'étudiant a déjà une note dans cette UE.");

            // Création de la note
            var noteToCreate = new Note
            {
                EtudiantId = etudiant.Id,
                UeId = ue.Id,
                Value = note
            };

            var createdNote = await _noteRepository.CreateAsync(noteToCreate);
            return createdNote;
        }
    }
}