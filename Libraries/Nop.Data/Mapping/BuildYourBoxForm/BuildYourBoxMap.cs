using Nop.Core.Domain.BuildYourBoxPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.BuildYourBoxForm
{
    public partial class BuildYourBoxMap : NopEntityTypeConfiguration<BuildYourBox>
    {
        public BuildYourBoxMap()
        {
            this.ToTable("BuildYourBox");
            this.HasKey(byb => byb.Id);

            this.Property(byb => byb.SubwooferManufacturer).HasMaxLength(100);
            this.Property(byb => byb.SubwooferModel).HasMaxLength(100);
            this.Property(byb => byb.NumberOfSubwoofers);
            this.Property(byb => byb.VehicleManufacturerOrModel).HasMaxLength(100);
            this.Property(byb => byb.VehicleYear).HasMaxLength(100);
            this.Property(byb => byb.WidthRequired).HasMaxLength(100);
            this.Property(byb => byb.HeightRequired).HasMaxLength(100);
            this.Property(byb => byb.DepthRequired).HasMaxLength(100);
            this.Property(byb => byb.CarpetColorId);
            this.Property(byb => byb.TerminalLocation).HasMaxLength(100);
            this.Property(byb => byb.TerminalTypeId);
            this.Property(byb => byb.SealedOrVented).HasMaxLength(100);
            this.Property(byb => byb.MDFThicknessId);
            this.Property(byb => byb.SquareOrAngledBox).HasMaxLength(100);

            this.Property(byb => byb.Name).HasMaxLength(100);
            this.Property(byb => byb.Address).HasMaxLength(500);
            this.Property(byb => byb.PhoneNumber).HasMaxLength(30);
            this.Property(byb => byb.Email).HasMaxLength(50);
        }
    }
}
