using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversiteDomain.Entities;

namespace UniversiteEFDataProvider.Data;
 
public class UniversiteDbContext : DbContext
{
    public static readonly ILoggerFactory consoleLogger = LoggerFactory.Create(builder => { builder.AddConsole(); });
    
    public UniversiteDbContext(DbContextOptions<UniversiteDbContext> options)
        : base(options)
    {
    }
 
    public UniversiteDbContext():base()
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(consoleLogger)  //on lie le contexte avec le système de journalisation
            .EnableSensitiveDataLogging() 
            .EnableDetailedErrors();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Propriétés de la table Etudiant
        modelBuilder.Entity<Etudiant>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<Etudiant>()
            .HasOne(e => e.ParcoursSuivi)
            .WithMany(p => p.Inscrits);
        modelBuilder.Entity<Etudiant>()
            .HasMany(e => e.NotesObtenues)
            .WithOne(n => n.Etudiant);

        // Propriétés de la table Parcours
        modelBuilder.Entity<Parcours>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Parcours>()
            .HasMany(p => p.Inscrits)
            .WithOne(e => e.ParcoursSuivi);
        modelBuilder.Entity<Parcours>()
            .HasMany(p => p.UesEnseignees)
            .WithMany(ue => ue.EnseigneeDans);

        // Propriétés de la table Ue
        modelBuilder.Entity<Ue>()
            .HasKey(ue => ue.Id);
        modelBuilder.Entity<Ue>()
            .HasMany(ue => ue.EnseigneeDans)
            .WithMany(p => p.UesEnseignees);
        modelBuilder.Entity<Ue>()
            .HasMany(ue => ue.Notes)
            .WithOne(n => n.notePourUe)
            .HasForeignKey(n => n.UeId);

        // Propriétés de la table Note
        modelBuilder.Entity<Note>()
            .HasKey(n => new { n.EtudiantId, n.UeId });
        modelBuilder.Entity<Note>()
            .HasOne(n => n.Etudiant)
            .WithMany(e => e.NotesObtenues)
            .HasForeignKey(n => n.EtudiantId);
        modelBuilder.Entity<Note>()
            .HasOne(n => n.notePourUe)
            .WithMany(ue => ue.Notes)
            .HasForeignKey(n => n.UeId);
    }
    public DbSet <Parcours>? Parcours { get; set; }
    public DbSet <Etudiant>? Etudiants { get; set; }
    public DbSet <Ue>? Ues { get; set; }
    public DbSet <Note>? Notes { get; set; }
}