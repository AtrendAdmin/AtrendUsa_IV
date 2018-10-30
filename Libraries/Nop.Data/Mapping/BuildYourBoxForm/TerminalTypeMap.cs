using Nop.Core.Domain.BuildYourBoxPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.BuildYourBoxForm
{
    public partial class TerminalTypeMap : NopEntityTypeConfiguration<TerminalTypes>
    {
        public TerminalTypeMap()
        {
            this.ToTable("TerminalType");
            this.HasKey(tt => tt.Id);

            this.Property(tt => tt.TerminalType).HasMaxLength(100);
            this.Property(tt => tt.ImageURL).HasMaxLength(500);
        }
    }
}
