using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookCollection.Models
{
    /// <summary>
    /// Book material type
    /// </summary>
    public enum Material
    {
        [Display(Name = "Hard Cover")]
        HardCover,
        [Display(Name = "Soft Cover")]
        SoftCover,
        [Display(Name = "Deluxe Edition")]
        Deluxe,
        [Display(Name = "Summary")]
        Summary,
        [Display(Name = "Pocket")]
        Pocket,
        [Display(Name = "Separate Box")]
        SeparateBox
    }

    public enum Condition
    {
        NotSpecified,
        New,
        Used,
        Poor
    }

    /// <summary>
    /// Book written language
    /// </summary>
    public enum Language
    {
        [Display(Name = "Dutch")]
        NL,
        [Display(Name = "German")]
        DE,
        [Display(Name = "English")]
        EN,
        [Display(Name = "Spanish")]
        ES,
        [Display(Name = "French")]
        FR,
        [Display(Name = "Italian")]
        IT,
        [Display(Name = "Danish")]
        DN,
        [Display(Name = "Other / Unknown")]
        Other
    }
}