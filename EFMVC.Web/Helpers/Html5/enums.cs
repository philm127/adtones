// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 10-08-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-08-2013
// ***********************************************************************
// <copyright file="enums.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/// <summary>
/// The Html namespace.
/// </summary>

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// Represents the type of the object to be rendered.
    /// </summary>
    public enum ObjectType
    {
        /// <summary>
        /// Represents the object tag containing the source as a flash file(.swf).
        /// </summary>
        Flash,

        /// <summary>
        /// Represents the object tag containing the source as a XAP file(.xap) which is used to play any video or audio in Silverlight.
        /// </summary>
        Silverlight
    }

    /// <summary>
    /// Represents the type of draw mode of a canvas or svg tag.
    /// </summary>
    internal enum DrawMode
    {
        /// <summary>
        /// Represents the fill type of the tag.
        /// </summary>
        Fill,

        /// <summary>
        /// Represents the stroke type of the tag.
        /// </summary>
        Stroke
    }
}