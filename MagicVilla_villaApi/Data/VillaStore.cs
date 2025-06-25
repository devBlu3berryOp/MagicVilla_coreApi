using MagicVilla_villaApi.Model.DTO;

namespace MagicVilla_villaApi.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList= new List<VillaDTO> { 
            new VillaDTO { Id=1,Name="Pool View",occupancy="double",sqft="1100"},
            new VillaDTO { Id=2,Name="Beach View",occupancy="triple",sqft="1400"}
            };
    }
}
