// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-22-2014
// ***********************************************************************
// <copyright file="QuestionOptionModel.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class QuestionOptionModel.
    /// </summary>
    public class QuestionOptionModel
    {
        /// <summary>
        /// Gets or sets the name of the question.
        /// </summary>
        /// <value>The name of the question.</value>
        public string QuestionName { get; set; }

        /// <summary>
        /// Gets or sets the question value.
        /// </summary>
        /// <value>The question value.</value>
        public string QuestionValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="QuestionOptionModel"/> is selected.
        /// </summary>
        /// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
        public bool Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [default answer].
        /// </summary>
        /// <value><c>true</c> if [default answer]; otherwise, <c>false</c>.</value>
        public bool DefaultAnswer { get; set; }
    }
}