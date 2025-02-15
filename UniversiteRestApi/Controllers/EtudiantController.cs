using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversiteApplication.UseCases.EtudiantUseCases.Get;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.EtudiantUseCases.Create;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Dtos;

namespace UniversiteRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtudiantController(IEtudiantRepository repositoryFactory) : ControllerBase
    {
        // GET: api/<EtudiantController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EtudiantController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<EtudiantDto>> GetUnEtudiant(string id)
        {
            var getEtudiantUc = new GetEtudiantByNumUseCase(repositoryFactory);
            var etudiant = await getEtudiantUc.ExecuteAsync(id);

            if (etudiant == null)
            {
                return NotFound();
            }

            return Ok(new EtudiantDto().ToDto(etudiant));
        }
        
        // POST api/<EtudiantController>
        [HttpPost]
        public async Task<ActionResult<EtudiantDto>> PostAsync([FromBody] EtudiantDto etudiantDto)
        {
            CreateEtudiantUseCase createEtudiantUc = new CreateEtudiantUseCase(repositoryFactory);           
            Etudiant etud = etudiantDto.ToEntity();
            try
            {
                etud = await createEtudiantUc.ExecuteAsync(etud);
            }
            catch (Exception e)
            {
                // On récupère ici les exceptions personnalisées définies dans la couche domain
                // Et on les envoie avec le code d'erreur 400 et l'intitulé "erreurs de validation"
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            EtudiantDto dto = new EtudiantDto().ToDto(etud);
            // On revoie la route vers le get qu'on n'a pas encore écrit!
            return CreatedAtAction(nameof(GetUnEtudiant), new { id = dto.Id }, dto);
        }

        // PUT api/<EtudiantController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EtudiantController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
