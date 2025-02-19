using System.ComponentModel.DataAnnotations;

namespace LojiteksWeb.Entities.Enums
{
    public enum CihazTipiEnum
    {
        [Display(Name = "SBox")]
        SBox = 0,
        [Display(Name = "Alarm Id")]
        AlarmId = 1,
    }
}
