namespace Accredia.GestioneAnagrafica.Shared.State
{
    /// <summary>
    /// Stato dell'utente autenticato
    /// </summary>
    public class UserState
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool IsAuthenticated { get; set; }

        public event Action? OnStateChanged;

        public void NotifyStateChanged()
        {
            OnStateChanged?.Invoke();
        }
    }
}
