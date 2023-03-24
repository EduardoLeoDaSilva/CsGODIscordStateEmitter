namespace CsGOStateEmitter.Entities
{
    public class GameScreemShots
    {
        public int Id { get; set; }
        public long SteamId { get; set; }
        public int MatchId { get; set; }
        public string ImageBase64 { get; set; }
        public DateTime CreatedDate { get; set; }

        public Player Players { get; set; }
    }
}
