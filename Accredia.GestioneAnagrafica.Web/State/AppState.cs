namespace Accredia.GestioneAnagrafica.Web.State
{
    /// <summary>
    /// Stato globale dell'applicazione
    /// </summary>
    public class AppState
    {
        public string? CurrentPage { get; set; }
        public bool IsLoading { get; set; }
        public string? ErrorMessage { get; set; }

        public event Action? OnStateChanged;

        public void NotifyStateChanged()
        {
            OnStateChanged?.Invoke();
        }
    }
}
