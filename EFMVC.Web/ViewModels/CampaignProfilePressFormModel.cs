// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-22-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-22-2014
// ***********************************************************************
// <copyright file="CampaignProfilePressFormModel.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class CampaignProfilePressFormModel.
    /// </summary>
    public class CampaignProfilePressFormModel : ArtharFormModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfilePressFormModel"/> class.
        /// </summary>
        public CampaignProfilePressFormModel()
        {
            LocalQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            NationalQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            FreeNewpapersQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            MagazinesQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
        }

        /// <summary>
        /// Gets or sets the campaign profile press identifier.
        /// </summary>
        /// <value>The campaign profile press identifier.</value>
        [Key]
        public int CampaignProfilePressId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile.
        /// </summary>
        /// <value>The campaign profile.</value>
        public CampaignProfileFormModel CampaignProfile { get; set; }

        /// <summary>
        /// Gets or sets the local question.
        /// </summary>
        /// <value>The local question.</value>
        [Display(Name = "Local")]
        public List<QuestionOptionModel> LocalQuestion { get; set; }

        /// <summary>
        /// Gets or sets the local.
        /// </summary>
        /// <value>The local.</value>
        [Display(Name = "Local")]
        public string Local_Press
        {
            get
            {
                if (LocalQuestion == null)
                    LocalQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(LocalQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    LocalQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the national question.
        /// </summary>
        /// <value>The national question.</value>
        [Display(Name = "National")]
        public List<QuestionOptionModel> NationalQuestion { get; set; }

        /// <summary>
        /// Gets or sets the national.
        /// </summary>
        /// <value>The national.</value>
        [Display(Name = "National")]
        public string National_Press
        {
            get
            {
                if (NationalQuestion == null)
                    NationalQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(NationalQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    NationalQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the free newpapers question.
        /// </summary>
        /// <value>The free newpapers question.</value>
        [Display(Name = "FreeNewpapers")]
        public List<QuestionOptionModel> FreeNewpapersQuestion { get; set; }

        /// <summary>
        /// Gets or sets the free newpapers.
        /// </summary>
        /// <value>The free newpapers.</value>
        [Display(Name = "FreeNewpapers")]
        public string FreeNewpapers_Press
        {
            get
            {
                if (FreeNewpapersQuestion == null)
                    FreeNewpapersQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(FreeNewpapersQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    FreeNewpapersQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the magazines question.
        /// </summary>
        /// <value>The magazines question.</value>
        [Display(Name = "Magazines")]
        public List<QuestionOptionModel> MagazinesQuestion { get; set; }

        /// <summary>
        /// Gets or sets the magazines.
        /// </summary>
        /// <value>The magazines.</value>
        [Display(Name = "Magazines")]
        public string Magazines_Press
        {
            get
            {
                if (MagazinesQuestion == null)
                    MagazinesQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(MagazinesQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    MagazinesQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }
    }
}