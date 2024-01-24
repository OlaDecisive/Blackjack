﻿// <auto-generated />
using System;
using Blackjack.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Blackjack.Model.Migrations
{
    [DbContext(typeof(BlackjackContext))]
    [Migration("20240124134509_AddStatusToGame")]
    partial class AddStatusToGame
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("Blackjack.Model.Card", b =>
                {
                    b.Property<int>("Suit")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Value")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("DeckId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("HandId")
                        .HasColumnType("TEXT");

                    b.HasKey("Suit", "Value");

                    b.HasIndex("DeckId");

                    b.HasIndex("HandId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Blackjack.Model.Deck", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Decks");
                });

            modelBuilder.Entity("Blackjack.Model.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Blackjack.Model.GameState", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("GameId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("GameStates");
                });

            modelBuilder.Entity("Blackjack.Model.Hand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Hands");
                });

            modelBuilder.Entity("Blackjack.Model.Card", b =>
                {
                    b.HasOne("Blackjack.Model.Deck", null)
                        .WithMany("Cards")
                        .HasForeignKey("DeckId");

                    b.HasOne("Blackjack.Model.Hand", null)
                        .WithMany("Cards")
                        .HasForeignKey("HandId");
                });

            modelBuilder.Entity("Blackjack.Model.GameState", b =>
                {
                    b.HasOne("Blackjack.Model.Game", null)
                        .WithMany("Rounds")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("Blackjack.Model.Deck", b =>
                {
                    b.Navigation("Cards");
                });

            modelBuilder.Entity("Blackjack.Model.Game", b =>
                {
                    b.Navigation("Rounds");
                });

            modelBuilder.Entity("Blackjack.Model.Hand", b =>
                {
                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}
