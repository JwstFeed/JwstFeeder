using System.ComponentModel.DataAnnotations;

namespace JwstScheduleProvider.DAL.EntityModels;

internal class EntityUrlModel
{
    [Key]
    public string Url { get; set; }

    public bool IsProcessed { get; set; }
}