namespace CsGOStateEmitter.Entities
{
    /// <summary>
    /// Entidade utilizada somente para o anticheating
    /// </summary>
    public class Player
    {
        public string SteamId { get; set; }
        public string Name { get; set; }
        public bool IsConnected { get; set; }
        public string Map { get; set; }
        public DateTime LastPhotoTaken { get; set; } = DateTime.Now.AddSeconds(30);
        public bool IsAntiCheatOpen { get; set; }
        public DateTime Expiration { get; set; }

        public bool CanITakePhoto()
        {
            var seconds = (LastPhotoTaken - DateTime.Now).Seconds;
            if (seconds <= 50)
                return true;

            return false;
        }
    }

    public class ServerStatus
    {
        public string Id { get; set; }
        public string Status { get; set; }
    }
}
