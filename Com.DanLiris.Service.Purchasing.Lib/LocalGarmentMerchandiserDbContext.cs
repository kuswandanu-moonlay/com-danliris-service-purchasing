using System;
using Com.DanLiris.Service.Purchasing.Lib.Models.LocalGarmentMerchandiserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Com.DanLiris.Service.Purchasing.Lib
{
    public partial class LocalGarmentMerchandiserDbContext : DbContext
    {
        public LocalGarmentMerchandiserDbContext(DbContextOptions<LocalGarmentMerchandiserDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Budget> Budget { get; set; }
        public virtual DbSet<Budget1> Budget1 { get; set; }
        public virtual DbSet<Porder> Porder { get; set; }
        public virtual DbSet<Porder1> Porder1 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Budget>(entity =>
            {
                entity.HasIndex(e => new { e.Harga, e.Po })
                    .HasName("_dta_index_Budget_15_446624634__K12_16");

                entity.HasIndex(e => new { e.Id, e.Urut })
                    .HasName("id_budget")
                    .IsUnique();

                entity.HasIndex(e => new { e.Codeby, e.Article, e.Ketr, e.Qty, e.Satb, e.Ro, e.Po })
                    .HasName("_dta_index_Budget_15_446624634__K3_K12_6_7_13_14_15");

                entity.HasIndex(e => new { e.Id, e.Urut, e.Konf, e.Seksi, e.Codeby, e.Article, e.Cat, e.Series, e.Kodeb, e.Kodec, e.Po, e.Ketr, e.Qty, e.Satb, e.Harga, e.Jumlah, e.Comp, e.Const, e.Yarn, e.Width, e.DtlsPo, e.ExMaster, e.ExPo, e.Userin, e.Tglin, e.Jamin, e.Usered, e.Tgled, e.Jamed, e.Ro })
                    .HasName("_dta_index_Budget_15_446624634__K3_1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_28_29_30");

                entity.HasIndex(e => new { e.Id, e.Urut, e.Konf, e.Seksi, e.Codeby, e.Article, e.Cat, e.Series, e.Kodec, e.Po, e.Ketr, e.Qty, e.Satb, e.Comp, e.Const, e.Yarn, e.Width, e.DtlsPo, e.ExMaster, e.ExPo, e.Userin, e.Tglin, e.Jamin, e.Usered, e.Tgled, e.Jamed, e.Kodeb, e.Ro, e.Jumlah, e.Harga })
                    .HasName("_dta_index_Budget_15_446624634__K10D_K3_K17_K16_1_2_4_5_6_7_8_9_11_12_13_14_15_18_19_20_21_22_23_24_25_26_27_28_29_30");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Article).HasMaxLength(25);

                entity.Property(e => e.Cat).HasMaxLength(3);

                entity.Property(e => e.Codeby).HasMaxLength(3);

                entity.Property(e => e.Comp).HasMaxLength(40);

                entity.Property(e => e.Const).HasMaxLength(30);

                entity.Property(e => e.DtlsPo)
                    .HasColumnName("Dtls_Po")
                    .HasMaxLength(200);

                entity.Property(e => e.ExMaster)
                    .HasColumnName("Ex_Master")
                    .HasMaxLength(1);

                entity.Property(e => e.ExPo)
                    .HasColumnName("Ex_Po")
                    .HasMaxLength(11);

                entity.Property(e => e.Harga).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Jamed).HasMaxLength(8);

                entity.Property(e => e.Jamin).HasMaxLength(8);

                entity.Property(e => e.Jumlah).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Ketr).HasMaxLength(50);

                entity.Property(e => e.Kodeb).HasMaxLength(9);

                entity.Property(e => e.Kodec).HasMaxLength(5);

                entity.Property(e => e.Konf).HasMaxLength(3);

                entity.Property(e => e.Po).HasMaxLength(11);

                entity.Property(e => e.Qty).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Ro).HasMaxLength(7);

                entity.Property(e => e.Satb).HasMaxLength(10);

                entity.Property(e => e.Seksi).HasMaxLength(1);

                entity.Property(e => e.Series).HasMaxLength(5);

                entity.Property(e => e.Tgled).HasColumnType("datetime");

                entity.Property(e => e.Tglin).HasColumnType("datetime");

                entity.Property(e => e.Urut).HasMaxLength(2);

                entity.Property(e => e.Usered).HasMaxLength(30);

                entity.Property(e => e.Userin).HasMaxLength(30);

                entity.Property(e => e.Width).HasMaxLength(7);

                entity.Property(e => e.Yarn).HasMaxLength(20);
            });

            modelBuilder.Entity<Budget1>(entity =>
            {
                entity.HasIndex(e => new { e.Cat, e.Ro, e.Konf, e.Article, e.Codeby, e.Satb, e.Seksi, e.Kodec, e.Jml, e.Harga })
                    .HasName("_dta_index_Budget1_15_878626173__K8_K3_K4_K7_K6_K20_K5_K11_K19_K21");

                entity.HasIndex(e => new { e.Id, e.Urut, e.Konf, e.Seksi, e.Codeby, e.Article, e.Series, e.Kodeb, e.Kodec, e.Po, e.Ketr, e.Kett, e.Kett2, e.Kett3, e.Kett4, e.Kett5, e.Jml, e.Satb, e.Satp, e.Konv, e.Ttl, e.Handling, e.Qty, e.Total, e.Comp, e.Const, e.Yarn, e.Width, e.DtlsPo, e.ExMaster, e.ExPo, e.Userin, e.Tglin, e.Jamin, e.Usered, e.Tgled, e.Jamed, e.JmlPakai, e.Jumlah, e.Ro, e.Harga, e.Cat })
                    .HasName("_dta_index_Budget1_15_878626173__K28_K3_K21_K8_1_2_4_5_6_7_9_10_11_12_13_14_15_16_17_18_19_20_22_23_24_25_26_27_29_30_31_32_33_");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Article).HasMaxLength(25);

                entity.Property(e => e.Cat).HasMaxLength(3);

                entity.Property(e => e.Codeby).HasMaxLength(3);

                entity.Property(e => e.Comp).HasMaxLength(40);

                entity.Property(e => e.Const).HasMaxLength(30);

                entity.Property(e => e.DtlsPo)
                    .HasColumnName("Dtls_Po")
                    .HasMaxLength(200);

                entity.Property(e => e.ExMaster)
                    .HasColumnName("Ex_Master")
                    .HasMaxLength(1);

                entity.Property(e => e.ExPo)
                    .HasColumnName("Ex_Po")
                    .HasMaxLength(10);

                entity.Property(e => e.Handling).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Harga).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Jamed).HasMaxLength(8);

                entity.Property(e => e.Jamin).HasMaxLength(8);

                entity.Property(e => e.Jml).HasColumnType("decimal(12, 3)");

                entity.Property(e => e.JmlPakai)
                    .HasColumnName("Jml_Pakai")
                    .HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Jumlah).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Ketr).HasMaxLength(50);

                entity.Property(e => e.Kett).HasMaxLength(50);

                entity.Property(e => e.Kett2).HasMaxLength(50);

                entity.Property(e => e.Kett3).HasMaxLength(50);

                entity.Property(e => e.Kett4).HasMaxLength(50);

                entity.Property(e => e.Kett5).HasMaxLength(50);

                entity.Property(e => e.Kodeb).HasMaxLength(9);

                entity.Property(e => e.Kodec).HasMaxLength(5);

                entity.Property(e => e.Konf).HasMaxLength(3);

                entity.Property(e => e.Konv).HasColumnType("decimal(12, 4)");

                entity.Property(e => e.Po).HasMaxLength(11);

                entity.Property(e => e.Qty).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Ro).HasMaxLength(8);

                entity.Property(e => e.Satb).HasMaxLength(10);

                entity.Property(e => e.Satp).HasMaxLength(10);

                entity.Property(e => e.Seksi).HasMaxLength(1);

                entity.Property(e => e.Series).HasMaxLength(5);

                entity.Property(e => e.Tgled).HasColumnType("datetime");

                entity.Property(e => e.Tglin).HasColumnType("datetime");

                entity.Property(e => e.Total).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Ttl).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Urut).HasMaxLength(2);

                entity.Property(e => e.Usered).HasMaxLength(30);

                entity.Property(e => e.Userin).HasMaxLength(30);

                entity.Property(e => e.Width).HasMaxLength(7);

                entity.Property(e => e.Yarn).HasMaxLength(20);
            });

            modelBuilder.Entity<Porder>(entity =>
            {
                entity.HasKey(e => e.IdPo);

                entity.ToTable("POrder");

                entity.HasIndex(e => e.IdPo)
                    .HasName("IX_POrder");

                entity.HasIndex(e => e.TgValid)
                    .HasName("indx_TgVld");

                entity.HasIndex(e => new { e.Tanggal, e.Post, e.CodeSpl, e.Harga })
                    .HasName("_dta_index_POrder_15_2114106572__K5D_K52_K7_K21_1_2_4_6_9_10_12_14_15_16_17_18_19_20_23_24_30_33_34_35_36_37_38_39_40_41_42_51_");

                entity.HasIndex(e => new { e.Ro, e.Konf, e.Kodeb, e.Ketr, e.Urut, e.Qty, e.Satb, e.Harga, e.Buyer, e.Art, e.Nopo })
                    .HasName("_dta_index_POrder_15_2114106572__K6_2_4_10_12_13_19_20_21_23_24");

                entity.HasIndex(e => new { e.IdPo, e.Po, e.Konf, e.Tanggal, e.CodeSpl, e.Series, e.Cat, e.Kodeb, e.Kodec, e.Ketr, e.Urut, e.Kett, e.Kett2, e.Kett3, e.Kett4, e.Kett5, e.Qty, e.Satb, e.Harga, e.HrgBeli, e.Buyer, e.Art, e.Tempo, e.Stat, e.Def, e.Jumlah, e.Tgl, e.Delivery, e.Seksi, e.Comp, e.Clr1, e.Clr2, e.Clr3, e.Clr4, e.Clr5, e.Clr6, e.Clr7, e.Clr8, e.Clr9, e.Clr10, e.Real, e.Revise, e.Term, e.Hari, e.Rcvd, e.Valas, e.Rate, e.Printed, e.Shipment, e.Post, e.DtlsPo, e.TgValid, e.TgPrint, e.Planed, e.JmlPersen, e.KetOb, e.FlagOb, e.MataUang, e.ShipDate, e.PlanDate, e.Deadline, e.Konv, e.Satk, e.Userin, e.Tglin, e.Jamin, e.Usered, e.Tgled, e.Jamed, e.Ro, e.Nopo })
                    .HasName("_dta_index_POrder_10_2114106572__K2_K6_1_3_4_5_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_28_29_30_31_32_33_");

                entity.HasIndex(e => new { e.IdPo, e.Ro, e.Po, e.Konf, e.Tanggal, e.CodeSpl, e.Series, e.Cat, e.Kodeb, e.Kodec, e.Ketr, e.Urut, e.Kett, e.Kett2, e.Kett3, e.Kett4, e.Kett5, e.Qty, e.Satb, e.Harga, e.HrgBeli, e.Buyer, e.Art, e.Tempo, e.Stat, e.Def, e.Jumlah, e.Tgl, e.Delivery, e.Seksi, e.Comp, e.Clr1, e.Clr2, e.Clr3, e.Clr4, e.Clr5, e.Clr6, e.Clr7, e.Clr8, e.Clr9, e.Clr10, e.Real, e.Revise, e.Term, e.Hari, e.Rcvd, e.Valas, e.Rate, e.Printed, e.Shipment, e.Post, e.DtlsPo, e.TgValid, e.TgPrint, e.Planed, e.JmlPersen, e.KetOb, e.FlagOb, e.MataUang, e.ShipDate, e.PlanDate, e.Deadline, e.Konv, e.Satk, e.Userin, e.Tglin, e.Jamin, e.Usered, e.Tgled, e.Jamed, e.Nopo })
                    .HasName("_dta_index_POrder_15_2114106572__K6_1_2_3_4_5_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_28_29_30_31_32_33_34_");

                entity.Property(e => e.IdPo)
                    .HasColumnName("ID_PO")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Art).HasMaxLength(25);

                entity.Property(e => e.Buyer).HasMaxLength(3);

                entity.Property(e => e.Cat).HasMaxLength(3);

                entity.Property(e => e.Clr1).HasMaxLength(50);

                entity.Property(e => e.Clr10).HasMaxLength(50);

                entity.Property(e => e.Clr2).HasMaxLength(50);

                entity.Property(e => e.Clr3).HasMaxLength(50);

                entity.Property(e => e.Clr4).HasMaxLength(50);

                entity.Property(e => e.Clr5).HasMaxLength(50);

                entity.Property(e => e.Clr6).HasMaxLength(50);

                entity.Property(e => e.Clr7).HasMaxLength(50);

                entity.Property(e => e.Clr8).HasMaxLength(50);

                entity.Property(e => e.Clr9).HasMaxLength(50);

                entity.Property(e => e.CodeSpl).HasMaxLength(5);

                entity.Property(e => e.Comp).HasMaxLength(40);

                entity.Property(e => e.Deadline).HasColumnType("datetime");

                entity.Property(e => e.Def).HasMaxLength(1);

                entity.Property(e => e.Delivery).HasColumnType("datetime");

                entity.Property(e => e.DtlsPo)
                    .HasColumnName("Dtls_po")
                    .HasMaxLength(200);

                entity.Property(e => e.FlagOb)
                    .HasColumnName("Flag_ob")
                    .HasMaxLength(1);

                entity.Property(e => e.Harga).HasColumnType("money");

                entity.Property(e => e.HrgBeli)
                    .HasColumnName("Hrg_Beli")
                    .HasColumnType("money");

                entity.Property(e => e.Jamed).HasMaxLength(8);

                entity.Property(e => e.Jamin).HasMaxLength(8);

                entity.Property(e => e.JmlPersen)
                    .HasColumnName("Jml_Persen")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Jumlah).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.KetOb)
                    .HasColumnName("Ket_ob")
                    .HasMaxLength(200);

                entity.Property(e => e.Ketr).HasMaxLength(50);

                entity.Property(e => e.Kett).HasMaxLength(50);

                entity.Property(e => e.Kett2).HasMaxLength(50);

                entity.Property(e => e.Kett3).HasMaxLength(50);

                entity.Property(e => e.Kett4).HasMaxLength(50);

                entity.Property(e => e.Kett5).HasMaxLength(50);

                entity.Property(e => e.Kodeb).HasMaxLength(9);

                entity.Property(e => e.Kodec).HasMaxLength(5);

                entity.Property(e => e.Konf)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.Konv).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.MataUang)
                    .HasColumnName("Mata_Uang")
                    .HasMaxLength(3);

                entity.Property(e => e.Nopo)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.PlanDate)
                    .HasColumnName("Plan_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Planed).HasColumnType("datetime");

                entity.Property(e => e.Po).HasMaxLength(15);

                entity.Property(e => e.Post).HasMaxLength(1);

                entity.Property(e => e.Printed).HasMaxLength(1);

                entity.Property(e => e.Qty).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Rate).HasColumnType("money");

                entity.Property(e => e.Rcvd).HasMaxLength(1);

                entity.Property(e => e.Real).HasMaxLength(1);

                entity.Property(e => e.Revise).HasMaxLength(1);

                entity.Property(e => e.Ro)
                    .IsRequired()
                    .HasMaxLength(7);

                entity.Property(e => e.Satb).HasMaxLength(10);

                entity.Property(e => e.Satk).HasMaxLength(5);

                entity.Property(e => e.Seksi).HasMaxLength(1);

                entity.Property(e => e.Series).HasMaxLength(5);

                entity.Property(e => e.ShipDate)
                    .HasColumnName("Ship_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Shipment).HasColumnType("datetime");

                entity.Property(e => e.Stat).HasMaxLength(15);

                entity.Property(e => e.Tanggal).HasColumnType("datetime");

                entity.Property(e => e.Tempo).HasColumnType("datetime");

                entity.Property(e => e.Term).HasMaxLength(1);

                entity.Property(e => e.TgPrint).HasColumnType("datetime");

                entity.Property(e => e.TgValid).HasColumnType("datetime");

                entity.Property(e => e.Tgl).HasColumnType("datetime");

                entity.Property(e => e.Tgled).HasColumnType("datetime");

                entity.Property(e => e.Tglin).HasColumnType("datetime");

                entity.Property(e => e.Urut).HasMaxLength(2);

                entity.Property(e => e.Usered).HasMaxLength(20);

                entity.Property(e => e.Userin).HasMaxLength(20);

                entity.Property(e => e.Valas).HasMaxLength(1);
            });

            modelBuilder.Entity<Porder1>(entity =>
            {
                entity.ToTable("POrder1");

                entity.HasIndex(e => new { e.CodeSpl, e.Harga, e.Tglin })
                    .HasName("idx_PoMaster");

                entity.HasIndex(e => new { e.Tgl, e.Ro, e.Kodeb })
                    .HasName("_dta_index_POrder1_15_830626002__K29_K2_K10");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Art).HasMaxLength(25);

                entity.Property(e => e.Buyer).HasMaxLength(3);

                entity.Property(e => e.Cat).HasMaxLength(3);

                entity.Property(e => e.Clr1).HasMaxLength(50);

                entity.Property(e => e.Clr10).HasMaxLength(50);

                entity.Property(e => e.Clr2).HasMaxLength(50);

                entity.Property(e => e.Clr3).HasMaxLength(50);

                entity.Property(e => e.Clr4).HasMaxLength(50);

                entity.Property(e => e.Clr5).HasMaxLength(50);

                entity.Property(e => e.Clr6).HasMaxLength(50);

                entity.Property(e => e.Clr7).HasMaxLength(50);

                entity.Property(e => e.Clr8).HasMaxLength(50);

                entity.Property(e => e.Clr9).HasMaxLength(50);

                entity.Property(e => e.CodeSpl).HasMaxLength(5);

                entity.Property(e => e.Comp).HasMaxLength(40);

                entity.Property(e => e.Deadline).HasColumnType("datetime");

                entity.Property(e => e.Def).HasMaxLength(1);

                entity.Property(e => e.Delivery).HasColumnType("datetime");

                entity.Property(e => e.DtlsPo)
                    .HasColumnName("Dtls_po")
                    .HasMaxLength(200);

                entity.Property(e => e.FlagOb)
                    .HasColumnName("Flag_ob")
                    .HasMaxLength(1);

                entity.Property(e => e.Harga).HasColumnType("money");

                entity.Property(e => e.HrgBeli)
                    .HasColumnName("Hrg_Beli")
                    .HasColumnType("money");

                entity.Property(e => e.IdUser)
                    .HasColumnName("Id_User")
                    .HasMaxLength(20);

                entity.Property(e => e.Jamed).HasMaxLength(8);

                entity.Property(e => e.Jamin).HasMaxLength(8);

                entity.Property(e => e.JmlPersen)
                    .HasColumnName("Jml_Persen")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Jumlah).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.KetOb).HasColumnName("Ket_ob");

                entity.Property(e => e.Ketr).HasMaxLength(50);

                entity.Property(e => e.Kett).HasMaxLength(50);

                entity.Property(e => e.Kett2).HasMaxLength(50);

                entity.Property(e => e.Kett3).HasMaxLength(50);

                entity.Property(e => e.Kett4).HasMaxLength(50);

                entity.Property(e => e.Kett5).HasMaxLength(50);

                entity.Property(e => e.Kodeb).HasMaxLength(9);

                entity.Property(e => e.Kodec).HasMaxLength(5);

                entity.Property(e => e.Konf).HasMaxLength(3);

                entity.Property(e => e.Konv).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.MataUang)
                    .HasColumnName("Mata_Uang")
                    .HasMaxLength(3);

                entity.Property(e => e.Nopo).HasMaxLength(11);

                entity.Property(e => e.PlanDate)
                    .HasColumnName("Plan_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Planed).HasColumnType("datetime");

                entity.Property(e => e.Po).HasMaxLength(15);

                entity.Property(e => e.Post).HasMaxLength(1);

                entity.Property(e => e.Printed).HasMaxLength(1);

                entity.Property(e => e.Qty).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Rate).HasColumnType("money");

                entity.Property(e => e.Rcvd).HasMaxLength(1);

                entity.Property(e => e.Real).HasMaxLength(1);

                entity.Property(e => e.Revise).HasMaxLength(1);

                entity.Property(e => e.Ro).HasMaxLength(8);

                entity.Property(e => e.Satb).HasMaxLength(10);

                entity.Property(e => e.Satk).HasMaxLength(5);

                entity.Property(e => e.Seksi).HasMaxLength(1);

                entity.Property(e => e.Series).HasMaxLength(5);

                entity.Property(e => e.ShipDate)
                    .HasColumnName("Ship_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Shipment).HasColumnType("datetime");

                entity.Property(e => e.Stat).HasMaxLength(15);

                entity.Property(e => e.Tanggal).HasColumnType("datetime");

                entity.Property(e => e.Tempo).HasColumnType("datetime");

                entity.Property(e => e.Term).HasMaxLength(1);

                entity.Property(e => e.TgPrint).HasColumnType("datetime");

                entity.Property(e => e.TgValid).HasColumnType("datetime");

                entity.Property(e => e.Tgl).HasColumnType("datetime");

                entity.Property(e => e.Tgled).HasColumnType("datetime");

                entity.Property(e => e.Tglin).HasColumnType("datetime");

                entity.Property(e => e.Urut).HasMaxLength(2);

                entity.Property(e => e.Usered).HasMaxLength(30);

                entity.Property(e => e.Userin).HasMaxLength(30);

                entity.Property(e => e.Valas).HasMaxLength(1);
            });
        }
    }
}
