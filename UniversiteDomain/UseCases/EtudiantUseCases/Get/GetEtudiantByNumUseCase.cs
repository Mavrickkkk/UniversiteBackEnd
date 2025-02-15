using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;

namespace UniversiteApplication.UseCases.EtudiantUseCases.Get;

public class GetEtudiantByNumUseCase(IEtudiantRepository etudiantRepository)
{
    private readonly IEtudiantRepository _etudiantRepository = etudiantRepository;

    public async Task<Etudiant?> ExecuteAsync(string numEtud)
    {
        return await _etudiantRepository.GetByNumEtudAsync(numEtud);
    }
}