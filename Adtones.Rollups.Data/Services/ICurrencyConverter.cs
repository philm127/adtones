namespace Adtones.Rollups.Data.Services
{
    public interface ICurrencyConverter
    {
        int DisplayCurrencyId { get; }
        decimal ConvertToDisplay(decimal value, int fromCurrencyId);
    }
}