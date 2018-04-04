using Nop.Core.Domain.Directory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.Directory
{
    public partial class ModelNumberMap : NopEntityTypeConfiguration<ModelNumber>
    {
        public ModelNumberMap()
        {
            this.ToTable("ModelNumber");
            this.HasKey(c => c.Id);
            this.Property(c => c.ModelNum).IsRequired().HasMaxLength(100);
        }
    }
}
