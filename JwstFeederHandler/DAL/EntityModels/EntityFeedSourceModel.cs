using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwstFeederHandler.DAL.EntityModels;

internal class EntityFeedSourceModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int SourceID { get; set; }

    public bool IsActive { get; set; }

    public int InputTypeID { get; set; }

    public string SourceName { get; set; }

    public int SourceTypeID { get; set; }

    public string Url { get; set; }
}