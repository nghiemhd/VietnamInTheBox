using Nop.Core.Domain.Aero87;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.Aero87
{
    public partial class POS_SaleOrderMap : EntityTypeConfiguration<POS_SaleOrder>
    {
        public POS_SaleOrderMap()
        {
            this.ToTable("POS_SaleOrder");
            this.HasKey(x => x.Id);
            this.Property(x => x.OrderNo).IsRequired().HasMaxLength(50);
            this.Property(x => x.OrderDate).IsRequired();
            this.Property(x => x.Desc);
            this.Property(x => x.ChanelId);
            this.Property(x => x.RefNo).HasMaxLength(50);
            this.Property(x => x.SubTotal);
            this.Property(x => x.ShippingFee);
            this.Property(x => x.Amount);
            this.Property(x => x.ReceiveMoney);
            this.Property(x => x.ReturnMoney);
            this.Property(x => x.CreatedUser).HasMaxLength(50);
            this.Property(x => x.CreatedDate);
            this.Property(x => x.UpdatedUser).HasMaxLength(50);
            this.Property(x => x.UpdatedDate);
            this.Property(x => x.VersionNo);
            this.Property(x => x.Mobile).HasMaxLength(50);
            this.Property(x => x.CustomerName).HasMaxLength(100);
            this.Property(x => x.DiscountPercent);
            this.Property(x => x.DiscountAmount);
            this.Property(x => x.Discount);
            this.Property(x => x.Status);
            this.Property(x => x.DebitAcctId);
            this.Property(x => x.ObjId);
            this.Property(x => x.FeeAcctId);
            this.Property(x => x.FeeAmount);
            this.Property(x => x.StoreId);
            this.Property(x => x.CarrierId);
            this.Property(x => x.AeroShippingFee);
            this.Property(x => x.PrintCount);
            this.Property(x => x.Users).HasMaxLength(50);
            this.Property(x => x.IsMasterCard);
            this.Property(x => x.SourceId);
            this.Property(x => x.ReturnReasonId);
            this.Property(x => x.ShippingCode).HasMaxLength(50);
            this.Property(x => x.IsPaid);
            this.Property(x => x.PaymentDate);
        }
    }
}
