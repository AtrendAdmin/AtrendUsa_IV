using Nop.Core.Domain.BuildYourBoxPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.BuildYourBoxForm
{
    public class CarpetColorMap : NopEntityTypeConfiguration<CarpetColors>
    {
        public CarpetColorMap()
        {
            this.ToTable("CarpetColor");
            this.HasKey(cc => cc.Id);

            this.Property(cc => cc.CarpetColor).HasMaxLength(100);
            this.Property(cc => cc.ImageURL).HasMaxLength(500);
        }
    }
}
