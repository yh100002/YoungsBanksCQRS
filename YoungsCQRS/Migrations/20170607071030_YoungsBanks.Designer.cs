using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using YoungsCQRS.ReadModel.Infrastructure;

namespace YoungsCQRS.Migrations
{
    [DbContext(typeof(AccountContext))]
    [Migration("20170607071030_YoungsBanks")]
    partial class YoungsBanks
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("YoungsCQRS.ReadModel.Dtos.Account", b =>
                {
                    b.Property<Guid>("AggregateID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountNumber");

                    b.Property<double>("Amount");

                    b.Property<string>("Currency")
                        .IsRequired();

                    b.Property<int>("Version");

                    b.HasKey("AggregateID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("YoungsCQRS.ReadModel.Dtos.EventStore", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("Eventid");

                    b.Property<DateTimeOffset>("TimeStamp");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("EventStorage");
                });
        }
    }
}
