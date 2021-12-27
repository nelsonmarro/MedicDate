using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedicDate.DataAccess.DbConverters;

public class TimeOnlyConverter : ValueConverter<TimeOnly, DateTime>
{
    public TimeOnlyConverter() : base(
        d => new DateTime(0, 0, 0, d.Hour, d.Minute, d.Second),
        d => TimeOnly.FromDateTime(d))
    { }
}
