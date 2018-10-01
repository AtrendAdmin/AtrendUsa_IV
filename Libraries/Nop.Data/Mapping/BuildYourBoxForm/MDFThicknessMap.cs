using Nop.Core.Domain.BuildYourBoxPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.BuildYourBoxForm
{
    public partial class MDFThicknessMap : NopEntityTypeConfiguration<MDFThicknesses>
    {
        public MDFThicknessMap()
        {
            this.ToTable("MDFThickness");
            this.HasKey(mdft => mdft.MDFThickness);

            this.Property(mdft => mdft.MDFThickness).HasMaxLength(100);
        }
    }
}
