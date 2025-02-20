using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.EtudiantUseCases.Create;
using UniversiteDomain.UseCases.NotesUseCases.Create;
using UniversiteDomain.UseCases.ParcoursUseCases.Create;
using UniversiteDomain.UseCases.ParcoursUseCases.EtudiantDansParcours;
using UniversiteDomain.UseCases.ParcoursUseCases.UeDansParcours;
using UniversiteDomain.UseCases.SecurityUseCases.Create;
using UniversiteDomain.UseCases.UeUseCases.Create;

namespace UniversiteDomain.JeuxDeDonnees;

public class BasicBdBuilder(IRepositoryFactory repositoryFactory) : BdBuilder(repositoryFactory)
{
    private readonly string Password = "Miage2025#";

    private readonly Etudiant[] _etudiants =
    [
        new Etudiant { Id=1,NumEtud = "03BDKZ65", Nom = "Bob", Prenom = "Leponge", Email = "bob.leponge@etud.u-picardie.fr" },
        new Etudiant { Id=2,NumEtud = "03BDKZ65", Nom = "Patrick", Prenom = "Letoile", Email = "patrick.letoile@etud.u-picardie.fr" },
    ];
    private struct UserNonEtudiant
    {
        public string UserName;
        public string Email;
        public string Role;
    }
    private readonly UserNonEtudiant[] _usersNonEtudiants =
    [
        new UserNonEtudiant { UserName = "marc.brasseur@u-picardie.fr", Email = "marc.brasseur@u-picardie.fr", Role = "Responsable" },
    ];

    private readonly Parcours[] _parcours =
    [
        new Parcours { Id=1,NomParcours = "M1", AnneeFormation = 1 },
        new Parcours { Id=2,NomParcours = "OSIE", AnneeFormation = 2 },
        new Parcours { Id=3,NomParcours = "ITD", AnneeFormation = 2 },
        new Parcours { Id=4,NomParcours = "INE", AnneeFormation = 2 }
    ];

    private readonly Ue[] _ues =
    [
        new Ue { Id=1, NumeroUe = "INFO_4", Intitule = "BDA" },
        new Ue { Id=2, NumeroUe = "TRANS_1", Intitule = "Anglais" },
        new Ue { Id=3, NumeroUe = "INFO_1", Intitule = "Marketing" },
    ];

    private struct Inscription
    {
        public long EtudiantId;
        public long ParcoursId;
    }

    private readonly Inscription[] _inscriptions =
    [
        // EtudiantId, ParcoursId
        new Inscription { EtudiantId = 1, ParcoursId = 2 },
        new Inscription { EtudiantId = 2, ParcoursId = 2 },
    ];

    private struct UeDansParcours
    {
        public long UeId;
        public long ParcoursId;
    }

    private readonly UeDansParcours[] _maquette =
    [
        new UeDansParcours { UeId = 1, ParcoursId = 1 },
        new UeDansParcours { UeId = 2, ParcoursId = 1 },
        new UeDansParcours { UeId = 3, ParcoursId = 1 },
    ];
    
    private struct Note
    {
        public long EtudiantId;
        public long UeId;
        public float Valeur;
    }
    
    private readonly Note[] _notes =
    [
        new Note { UeId = 1, EtudiantId = 2, Valeur = 12 },
        new Note { UeId = 2, EtudiantId = 2, Valeur = 14 },
        new Note { UeId = 4, EtudiantId = 1, Valeur = 10 },
    ];
    protected override async Task RegenererBdAsync()
    {
        // Ici je décide de supprimer et recréer la BD
        await repositoryFactory.EnsureDeletedAsync();
        await repositoryFactory.EnsureCreatedAsync();
    }
    protected override async Task BuildEtudiantsAsync()
    {
        foreach (Etudiant e in _etudiants)
        {
            await new CreateEtudiantUseCase(repositoryFactory).ExecuteAsync(e);
        }
    }
    protected override async Task BuildParcoursAsync()
    {
        foreach (Parcours parcours in _parcours)
        {
            await new CreateParcoursUseCase(repositoryFactory).ExecuteAsync(parcours);
        }
    }
    protected override async Task BuildUesAsync()
    {
        foreach (Ue ue in _ues)
        {
            await new CreateUeUseCase(repositoryFactory).ExecuteAsync(ue);
        }
    }

    protected override async Task InscrireEtudiantsAsync()
    {
        foreach (Inscription i in _inscriptions)
        {
            await new AddEtudiantDansParcoursUseCase(repositoryFactory).ExecuteAsync(i.ParcoursId,i.EtudiantId);
        }
    }
    protected override async Task BuildMaquetteAsync()
    {
        foreach(UeDansParcours u in _maquette)
        {
            await new AddUeDansParcoursUseCase(repositoryFactory).ExecuteAsync(u.ParcoursId, u.UeId);
        }
    }

    protected override async Task NoterAsync()
    {
        foreach( var note in _notes)
        {
            await new CreateNotesUseCase(repositoryFactory).ExecuteAsync(note.Valeur, note.EtudiantId,note.UeId);
        }
    }
    
    protected override async Task BuildRolesAsync()
    {
        await new CreateUniversiteRoleUseCase(repositoryFactory).ExecuteAsync(Roles.Responsable);
        await new CreateUniversiteRoleUseCase(repositoryFactory).ExecuteAsync(Roles.Scolarite);
        await new CreateUniversiteRoleUseCase(repositoryFactory).ExecuteAsync(Roles.Etudiant);
    }

    protected override async Task BuildUsersAsync()
    {
        CreateUniversiteUserUseCase uc = new CreateUniversiteUserUseCase(repositoryFactory);
        foreach (var etudiant in _etudiants)
        {
            await uc.ExecuteAsync(etudiant.Email, etudiant.Email, this.Password, Roles.Etudiant,etudiant);
        }
        
        foreach (var user in _usersNonEtudiants)
        {
            await uc.ExecuteAsync(user.Email, user.Email, this.Password, user.Role, null);
        }
    }
}