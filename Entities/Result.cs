namespace CsGOStateEmitter.Entities
{
    public class Result
    {
        public int MatchId { get; set; }
        public int MapNumber { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Winner { get; set; }
        public string MapName { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
    }
}
