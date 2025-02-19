using System.ComponentModel.DataAnnotations;

namespace LojiteksWeb.Entities.Enums
{
    public enum YetkiEnum
    {
        [Display(Name = "Admin")]
        Admin = 0,
        [Display(Name = "HandleTerminal")]
        HandleTerminal = 1,
        [Display(Name = "Report")]
        Report = 2
    }
}
