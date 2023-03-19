namespace CsGOStateEmitter.Entities
{
    public class PlayerStats
    {
        public int MatchId { get; set; }
        public int MapNumber { get; set; }
        public long SteamId64 { get; set; }
        public int RoundsPlayed { get; set; }
        public string Team { get; set; }
        public string Name { get; set; }

        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public int FlashBangAssists { get; set; }
        public int TeamKills { get; set; }
        public int KnifeKills { get; set; }
        public int HeadshotKills { get; set; }
        public int Damage { get; set; }
        public int UtilityDamage { get; set; }
        public int EnemiesFlashed { get; set; }
        public int FrindliesFlashed { get; set; }
        public int BombPlants { get; set; }
        public int BombDefuses { get; set; }
        public int V1 { get; set; }
        public int V2 { get; set; }
        public int V3 { get; set; }
        public int V4 { get; set; }
        public int V5 { get; set; }

        public int TwoK { get; set; }
        public int ThreeK { get; set; }
        public int FourK { get; set; }
        public int FiveK { get; set; }
        public int FirstkillT { get; set; }
        public int FirstkillCt { get; set; }
        public int FirstDeathT { get; set; }
        public int FirstDeathCt { get; set; }
        public int TradeKill { get; set; }
        public int Kast { get; set; }
        public int ContributionScore { get; set; }
        public int Mvp { get; set; }

    }
}
