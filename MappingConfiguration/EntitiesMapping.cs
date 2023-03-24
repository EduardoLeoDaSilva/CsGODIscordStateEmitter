using CsGOStateEmitter.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CsGOStateEmitter.MappingConfiguration
{
    public class EntitiesMapping
    {
        internal static Action<EntityTypeBuilder<Player>> ConfigurePlayers()
        {
            return entity =>
            {
                entity.ToTable("PlayersAntiCheating");
                entity.HasKey(m => m.SteamId);
                entity.Property(m => m.Name).IsRequired(true).HasMaxLength(100);
                entity.Property(m => m.Expiration);
                entity.Property(m => m.Map);
                entity.Property(m => m.IsConnected);
                entity.Property(m => m.IsAntiCheatOpen);
            };
        }

        internal static Action<EntityTypeBuilder<DiscordUser>> ConfigureDiscordUser()
        {
            return entity =>
            {
                entity.HasKey(x => x.DiscordId);
                entity.Property(x => x.SteamID);
                entity.Property(x => x.Name);
            };
        }

        internal static Action<EntityTypeBuilder<AdminBot>> ConfigureAdminBot()
        {
            return entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name);
            };
        }

        internal static Action<EntityTypeBuilder<Result>> ConfigureMatch()
        {
            return entity =>
            {
                entity.ToTable("get5_stats_maps");
                entity.HasKey(x => x.MatchId);
                entity.Property(x => x.MatchId).HasColumnName("matchid"); ;
                entity.Property(x => x.MapNumber).HasColumnName("mapnumber"); ;
                entity.Property(x => x.StartTime).HasColumnName("start_time");
                entity.Property(x => x.EndTime).HasColumnName("end_time");
                entity.Property(x => x.Winner).HasColumnName("winner");
                entity.Property(x => x.MapName).HasColumnName("mapname");
                entity.Property(x => x.Team1Score).HasColumnName("team1_score");
                entity.Property(x => x.Team2Score).HasColumnName("team2_score");
            };
        }


        internal static Action<EntityTypeBuilder<PlayerStats>> ConfigurePlayer()
        {
            return entity =>
            {
                entity.ToTable("get5_stats_players");

                entity.HasKey(x => new { x.MatchId, x.MapNumber, x.SteamId64 }); ;
                entity.Property(x => x.MatchId).HasColumnName("matchid");
                entity.Property(x => x.MapNumber).HasColumnName("mapnumber");
                entity.Property(x => x.SteamId64).HasColumnName("steamid64");
                entity.Property(x => x.Team).HasColumnName("team");
                entity.Property(x => x.RoundsPlayed).HasColumnName("rounds_played");
                entity.Property(x => x.Name).HasColumnName("name");
                entity.Property(x => x.Kills).HasColumnName("kills");
                entity.Property(x => x.Deaths).HasColumnName("deaths");
                entity.Property(x => x.Assists).HasColumnName("assists");
                entity.Property(x => x.FlashBangAssists).HasColumnName("flashbang_assists");
                entity.Property(x => x.TeamKills).HasColumnName("teamkills");
                entity.Property(x => x.KnifeKills).HasColumnName("knife_kills");
                entity.Property(x => x.HeadshotKills).HasColumnName("headshot_kills");
                entity.Property(x => x.Damage).HasColumnName("damage");
                entity.Property(x => x.UtilityDamage).IsRequired().HasColumnName("utility_damage");
                entity.Property(x => x.EnemiesFlashed).HasColumnName("enemies_flashed");
                entity.Property(x => x.FrindliesFlashed).HasColumnName("friendlies_flashed");
                entity.Property(x => x.BombPlants).HasColumnName("bomb_plants");
                entity.Property(x => x.BombDefuses).HasColumnName("bomb_defuses");
                entity.Property(x => x.V1).HasColumnName("v1");
                entity.Property(x => x.V2).HasColumnName("v2");
                entity.Property(x => x.V3).HasColumnName("v3");
                entity.Property(x => x.V4).HasColumnName("v4");
                entity.Property(x => x.V5).HasColumnName("v5");
                entity.Property(x => x.TwoK).HasColumnName("2k");
                entity.Property(x => x.ThreeK).HasColumnName("3k");
                entity.Property(x => x.FourK).HasColumnName("4k");
                entity.Property(x => x.FiveK).HasColumnName("5k");

                entity.Property(x => x.FirstkillCt).HasColumnName("firstkill_ct");
                entity.Property(x => x.FirstkillT).HasColumnName("firstkill_t");
                entity.Property(x => x.FirstDeathCt).HasColumnName("firstdeath_ct");
                entity.Property(x => x.FirstDeathT).HasColumnName("firstdeath_t");
                entity.Property(x => x.TradeKill).HasColumnName("tradekill");
                entity.Property(x => x.Kast).HasColumnName("kast");
                entity.Property(x => x.ContributionScore).HasColumnName("contribution_score");
                entity.Property(x => x.Mvp).HasColumnName("mvp");
                entity.Property(x => x.FiveK).HasColumnName("5k");

            };
        }
    }
}
