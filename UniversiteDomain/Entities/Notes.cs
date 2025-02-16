namespace UniversiteDomain.Entities;

public class Notes
{
    public long Id { get; set; }
    public float Valeur { get; set; }
    public long EtudiantId { get; set; }
    public long UeId { get; set; }
    public Etudiant? Etudiant { get; set; }
    public Ue? Ue { get; set; }
    
    public override string ToString()
    {
        return $"Value {Valeur} ";
    }
}