using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.BuildYourBoxPlugin
{
    /// <summary>
    /// Represents Custom Build Box Form Model
    /// </summary>
    public partial class BuildYourBox : BaseEntity
    {
        /// <summary>
        /// Gets or Sets SubwooferManufacturer property
        /// </summary>
        public string SubwooferManufacturer { get; set; }

        /// <summary>
        /// Gets or Sets SubwooferModel property
        /// </summary>
        public string SubwooferModel { get; set; }

        /// <summary>
        /// Gets or Sets NumberOfSubwoofers property
        /// </summary>
        public int NumberOfSubwoofers { get; set; }

        /// <summary>
        /// Gets or Sets VehicleManufacturerOrModel property
        /// </summary>
        public string VehicleManufacturerOrModel { get; set; }

        /// <summary>
        /// Gets or Sets VehicleYear property
        /// </summary>
        public string VehicleYear { get; set; }

        /// <summary>
        /// Gets or Sets WidthRequired property
        /// </summary>
        public string WidthRequired { get; set; }

        /// <summary>
        /// Gets or Sets HeightRequired property
        /// </summary>
        public string HeightRequired { get; set; }

        /// <summary>
        /// Gets or Sets DepthRequired property
        /// </summary>
        public string DepthRequired { get; set; }

        /// <summary>
        /// Gets or Sets CarpetColorId property
        /// </summary>
        public int CarpetColorId { get; set; }

        /// <summary>
        /// Gets or Sets TerminalLocation property
        /// </summary>
        public string TerminalLocation { get; set; }

        /// <summary>
        /// Gets or Sets TerminalTypeId property
        /// </summary>
        public int TerminalTypeId { get; set; }

        /// <summary>
        /// Gets or Sets SealedOrVented property
        /// </summary>
        public string SealedOrVented { get; set; }

        /// <summary>
        /// Gets or Sets MDFThicknessRequested property
        /// </summary>
        public int MDFThicknessId { get; set; }

        /// <summary>
        /// Gets or Sets SquareOrAngledBox property
        /// </summary>
        public string SquareOrAngledBox { get; set; }

        /// <summary>
        /// Gets or Sets Name property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Address property
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or Sets PhoneNumber property
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or Sets Email property
        /// </summary>
        public string Email { get; set; }
    }
}
