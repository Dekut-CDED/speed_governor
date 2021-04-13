using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Api.Pages.Admin.SpeedGovernors.Map
{
    public class MapModel : PageModel
    {
        public double latitude;
        public double lo;
        public void OnGet(double lat, double longitude)
        {
            latitude = lat;
            lo = longitude;
        }
    }
}
