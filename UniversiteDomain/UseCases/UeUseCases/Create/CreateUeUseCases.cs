using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.UeExceptions;

namespace UniversiteDomain.UseCases.UeUseCases.Create
{
    public class CreateUeUseCase
    {
        private readonly IUeRepository _ueRepository;

        public CreateUeUseCase(IUeRepository ueRepository)
        {
            _ueRepository = ueRepository;
        }

        public async Task<Ue> ExecuteAsync(string numeroUe, string intitule)
        {
            var ue = new Ue { NumeroUe = numeroUe, Intitule = intitule };

            await CheckBusinessRules(ue);

            var createdUe = await _ueRepository.CreateAsync(ue);
            return createdUe;
        }

        private async Task CheckBusinessRules(Ue ue)
        {
            var existingUe = await _ueRepository.FindByConditionAsync(u => u.NumeroUe == ue.NumeroUe);
            if (existingUe.Any())
            {
                throw new DuplicateUeDansParcoursException("Unité d'enseignement avec ce numéro existe déjà.");
            }

            if (string.IsNullOrEmpty(ue.Intitule) || ue.Intitule.Length < 3)
            {
                throw new UeNotFoundException("L'intitulé de l'UE doit contenir au moins 3 caractères.");
            }
        }
    }
}