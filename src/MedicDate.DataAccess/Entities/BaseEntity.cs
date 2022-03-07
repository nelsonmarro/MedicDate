namespace MedicDate.DataAccess.Entities;

public class BaseEntity
{
    public DateTime DateRegistered { get; set; }
    public string RegisterBy { get; set; } = string.Empty;
    public DateTime DateModify { get; set; }
    public string ModifyBy { get; set; } = string.Empty;
}
