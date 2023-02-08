using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwstScheduleProvider.DAL.EntityModels;

internal class EntityLogModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int LogID { get; set; }

    public string Log { get; set; }
}