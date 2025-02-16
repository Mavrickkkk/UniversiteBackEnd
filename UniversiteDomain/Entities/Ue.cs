namespace UniversiteDomain.Entities;

public class Ue
{
    public long Id { get; set; }
    public string NumeroUe { get; set; } = String.Empty;
    public string Intitule { get; set; } = String.Empty;
    
    // ManyToMany
    // Une Ue peut être enseignée dans différents parcours
    public List<Parcours>? EnseigneeDans { get; set; } = new();
    
    // OneToMany
    // Une UE contiens plusieurs notes
    public List<Notes> Notes { get; set; } = new();
    
    public override string ToString()
    {
        return "ID "+Id +" : "+NumeroUe+" - "+Intitule;
    }
}