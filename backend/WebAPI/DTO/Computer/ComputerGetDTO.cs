using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO.Computer
{
    public class ComputerGetDTO
    {
        public string Id { get; set; } = null!;

        public string Mainboard { get; set; } = null!;

        public string GPU { get; set; } = null!;

        public string CPU { get; set; } = null!;

        public string Ram { get; set; } = null!;

        public string Ssd { get; set; } = null!;

        public string PowerSupply { get; set; } = null!;

        public string Cooler { get; set; } = null!;

        public string Case { get; set; } = null!;
         
        public double Price { get; set; }
    }
}
