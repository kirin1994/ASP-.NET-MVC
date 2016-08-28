using MvcProject.Models;
using System.Collections.Generic;

namespace MvcProject.ViewModels
{

    public class HomeViewModel
    {
         public bool ShowActions { get; set; }
         public IEnumerable<Gig> UpcomingGigs { get; set; }
         public string Heading { get; set; }
    }

}