// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-22-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-22-2014
// ***********************************************************************
// <copyright file="CampaignProfileInternetFormModel.cs" company="Noat">
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
    /// Class CampaignProfileInternetFormModel.
    /// </summary>
    public class CampaignProfileInternetFormModel : ArtharFormModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileInternetFormModel"/> class.
        /// </summary>
        public CampaignProfileInternetFormModel()
        {
            SocialNetworkingQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            VideoQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            ResearchQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            AuctionsQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            ShoppingQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
        }

        /// <summary>
        /// Gets or sets the campaign profile internet identifier.
        /// </summary>
        /// <value>The campaign profile internet identifier.</value>
        [Key]
        public int CampaignProfileInternetId { get; set; }

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
        /// Gets or sets the social networking question.
        /// </summary>
        /// <value>The social networking question.</value>
        [Display(Name = "Social Networking")]
        public List<QuestionOptionModel> SocialNetworkingQuestion { get; set; }

        /// <summary>
        /// Gets or sets the social networking.
        /// </summary>
        /// <value>The social networking.</value>
        [Display(Name = "Social Networking")]
        public string SocialNetworking_Internet
        {
            get
            {
                if (SocialNetworkingQuestion == null)
                    SocialNetworkingQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(SocialNetworkingQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    SocialNetworkingQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the video question.
        /// </summary>
        /// <value>The video question.</value>
        [Display(Name = "Video")]
        public List<QuestionOptionModel> VideoQuestion { get; set; }

        /// <summary>
        /// Gets or sets the video.
        /// </summary>
        /// <value>The video.</value>
        [Display(Name = "Video")]
        public string Video_Internet
        {
            get
            {
                if (VideoQuestion == null)
                    VideoQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(VideoQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    VideoQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the research question.
        /// </summary>
        /// <value>The research question.</value>
        [Display(Name = "Research")]
        public List<QuestionOptionModel> ResearchQuestion { get; set; }

        /// <summary>
        /// Gets or sets the research.
        /// </summary>
        /// <value>The research.</value>
        [Display(Name = "Research")]
        public string Research_Internet
        {
            get
            {
                if (ResearchQuestion == null)
                    ResearchQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(ResearchQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    ResearchQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the auctions question.
        /// </summary>
        /// <value>The auctions question.</value>
        [Display(Name = "Auctions")]
        public List<QuestionOptionModel> AuctionsQuestion { get; set; }

        /// <summary>
        /// Gets or sets the auctions.
        /// </summary>
        /// <value>The auctions.</value>
        [Display(Name = "Auctions")]
        public string Auctions_Internet
        {
            get
            {
                if (AuctionsQuestion == null)
                    AuctionsQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(AuctionsQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    AuctionsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the shopping question.
        /// </summary>
        /// <value>The shopping question.</value>
        [Display(Name = "Shopping")]
        public List<QuestionOptionModel> ShoppingQuestion { get; set; }

        /// <summary>
        /// Gets or sets the shopping.
        /// </summary>
        /// <value>The shopping.</value>
        [Display(Name = "Shopping")]
        public string Shopping_Internet
        {
            get
            {
                if (ShoppingQuestion == null)
                    ShoppingQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(ShoppingQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    ShoppingQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }
    }
}