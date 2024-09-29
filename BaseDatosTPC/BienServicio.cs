using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    public class BienServicio
    {

        [Key] 
        public int ID_Bien_Servicio {  get; set; }
        public string? Bien_Servicio { get; set;}

        
    }
}
