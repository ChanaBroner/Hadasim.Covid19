namespace Covid_19.Controllers
{
    public class EmployeeVaccination
    {
        public string IdentityCard { get; set; }
        public DateTime? Vaccine1Date { get; set; }
        public string Vaccine1Manufacturer { get; set; }
        public DateTime? Vaccine2Date { get; set; }
        public string Vaccine2Manufacturer { get; set; }
        public DateTime? Vaccine3Date { get; set; }
        public string? Vaccine3Manufacturer { get; set; } // Make it nullable string
        public DateTime? Vaccine4Date { get; set; }
        public string? Vaccine4Manufacturer { get; set; }
        public DateTime? PositiveResultDate { get; set; }
        public DateTime? RecoveryDate { get; set; }
    }
}
