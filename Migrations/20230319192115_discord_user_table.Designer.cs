﻿// <auto-generated />
using System;
using CsGOStateEmitter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CsGOStateEmitter.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230319192115_discord_user_table")]
    partial class discord_user_table
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CsGOStateEmitter.Entities.DiscordUser", b =>
                {
                    b.Property<string>("DiscordId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("SteamID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("DiscordId");

                    b.ToTable("DiscordUser");
                });

            modelBuilder.Entity("CsGOStateEmitter.Entities.PlayerStats", b =>
                {
                    b.Property<int>("MatchId")
                        .HasColumnType("int")
                        .HasColumnName("matchid");

                    b.Property<int>("MapNumber")
                        .HasColumnType("int")
                        .HasColumnName("mapnumber");

                    b.Property<long>("SteamId64")
                        .HasColumnType("bigint")
                        .HasColumnName("steamid64");

                    b.Property<int>("Assists")
                        .HasColumnType("int")
                        .HasColumnName("assists");

                    b.Property<int>("BombDefuses")
                        .HasColumnType("int")
                        .HasColumnName("bomb_defuses");

                    b.Property<int>("BombPlants")
                        .HasColumnType("int")
                        .HasColumnName("bomb_plants");

                    b.Property<int>("ContributionScore")
                        .HasColumnType("int")
                        .HasColumnName("contribution_score");

                    b.Property<int>("Damage")
                        .HasColumnType("int")
                        .HasColumnName("damage");

                    b.Property<int>("Deaths")
                        .HasColumnType("int")
                        .HasColumnName("deaths");

                    b.Property<int>("EnemiesFlashed")
                        .HasColumnType("int")
                        .HasColumnName("enemies_flashed");

                    b.Property<int>("FirstDeathCt")
                        .HasColumnType("int")
                        .HasColumnName("firstdeath_ct");

                    b.Property<int>("FirstDeathT")
                        .HasColumnType("int")
                        .HasColumnName("firstdeath_t");

                    b.Property<int>("FirstkillCt")
                        .HasColumnType("int")
                        .HasColumnName("firstkill_ct");

                    b.Property<int>("FirstkillT")
                        .HasColumnType("int")
                        .HasColumnName("firstkill_t");

                    b.Property<int>("FiveK")
                        .HasColumnType("int")
                        .HasColumnName("5k");

                    b.Property<int>("FlashBangAssists")
                        .HasColumnType("int")
                        .HasColumnName("flashbang_assists");

                    b.Property<int>("FourK")
                        .HasColumnType("int")
                        .HasColumnName("4k");

                    b.Property<int>("FrindliesFlashed")
                        .HasColumnType("int")
                        .HasColumnName("friendlies_flashed");

                    b.Property<int>("HeadshotKills")
                        .HasColumnType("int")
                        .HasColumnName("headshot_kills");

                    b.Property<int>("Kast")
                        .HasColumnType("int")
                        .HasColumnName("kast");

                    b.Property<int>("Kills")
                        .HasColumnType("int")
                        .HasColumnName("kills");

                    b.Property<int>("KnifeKills")
                        .HasColumnType("int")
                        .HasColumnName("knife_kills");

                    b.Property<int>("Mvp")
                        .HasColumnType("int")
                        .HasColumnName("mvp");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<int>("RoundsPlayed")
                        .HasColumnType("int")
                        .HasColumnName("rounds_played");

                    b.Property<string>("Team")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("team");

                    b.Property<int>("TeamKills")
                        .HasColumnType("int")
                        .HasColumnName("teamkills");

                    b.Property<int>("ThreeK")
                        .HasColumnType("int")
                        .HasColumnName("3k");

                    b.Property<int>("TradeKill")
                        .HasColumnType("int")
                        .HasColumnName("tradekill");

                    b.Property<int>("TwoK")
                        .HasColumnType("int")
                        .HasColumnName("2k");

                    b.Property<int>("UtilityDamage")
                        .HasColumnType("int")
                        .HasColumnName("utility_damage");

                    b.Property<int>("V1")
                        .HasColumnType("int")
                        .HasColumnName("v1");

                    b.Property<int>("V2")
                        .HasColumnType("int")
                        .HasColumnName("v2");

                    b.Property<int>("V3")
                        .HasColumnType("int")
                        .HasColumnName("v3");

                    b.Property<int>("V4")
                        .HasColumnType("int")
                        .HasColumnName("v4");

                    b.Property<int>("V5")
                        .HasColumnType("int")
                        .HasColumnName("v5");

                    b.HasKey("MatchId", "MapNumber", "SteamId64");

                    b.ToTable("get5_stats_players", (string)null);
                });

            modelBuilder.Entity("CsGOStateEmitter.Entities.Result", b =>
                {
                    b.Property<int>("MatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("matchid");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("end_time");

                    b.Property<string>("MapName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("mapname");

                    b.Property<int>("MapNumber")
                        .HasColumnType("int")
                        .HasColumnName("mapnumber");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("start_time");

                    b.Property<int>("Team1Score")
                        .HasColumnType("int")
                        .HasColumnName("team1_score");

                    b.Property<int>("Team2Score")
                        .HasColumnType("int")
                        .HasColumnName("team2_score");

                    b.Property<string>("Winner")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("winner");

                    b.HasKey("MatchId");

                    b.ToTable("get5_stats_maps", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
