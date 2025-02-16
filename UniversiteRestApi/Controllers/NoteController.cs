using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.SecurityUseCases.Get;

namespace UniversiteRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController(IRepositoryFactory repositoryFactory) : ControllerBase
    {
        private void CheckSecu(out string role, out string email, out IUniversiteUser user)
        {
            role = "";
            ClaimsPrincipal claims = HttpContext.User;
            if (claims.FindFirst(ClaimTypes.Email) == null) throw new UnauthorizedAccessException();
            email = claims.FindFirst(ClaimTypes.Email).Value;
            if (email == null) throw new UnauthorizedAccessException();
            
            user = new FindUniversiteUserByEmailUseCase(repositoryFactory).ExecuteAsync(email).Result;
            if (user == null) throw new UnauthorizedAccessException();
            if (claims.Identity?.IsAuthenticated != true) throw new UnauthorizedAccessException();
            var ident = claims.Identities.FirstOrDefault();
            if (ident == null) throw new UnauthorizedAccessException();
            if (claims.FindFirst(ClaimTypes.Role) == null) throw new UnauthorizedAccessException();
            role = ident.FindFirst(ClaimTypes.Role).Value;
            if (role == null) throw new UnauthorizedAccessException();
        }
    }
}