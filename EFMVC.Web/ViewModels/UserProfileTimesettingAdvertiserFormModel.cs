// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-23-2014
// ***********************************************************************
// <copyright file="UserProfileAdvertFormModel.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class UserProfileAdvertFormModel.
    /// </summary>
    public class UserProfileTimesettingAdvertiserFormModel
    {
        public string Timesetting_Name { get; set; }
        public List<string> Timesetting_Name_Label { get; set; }

        public IEnumerable<TimeOfDay> MondaySelectedTimes { get; set; }

        public IEnumerable<TimeOfDay> TuesdaySelectedTimes { get; set; }

        public IEnumerable<TimeOfDay> WednesdaySelectedTimes { get; set; }

        public IEnumerable<TimeOfDay> ThursdaySelectedTimes { get; set; }

        public IEnumerable<TimeOfDay> FridaySelectedTimes { get; set; }

        public IEnumerable<TimeOfDay> SaturdaySelectedTimes { get; set; }

        public IEnumerable<TimeOfDay> SundaySelectedTimes { get; set; }

        public UserProfileTimesettingAdvertiserFormModel()
        {
            MondaySelectedTimes = new List<TimeOfDay>();
            TuesdaySelectedTimes = new List<TimeOfDay>();
            WednesdaySelectedTimes = new List<TimeOfDay>();
            ThursdaySelectedTimes = new List<TimeOfDay>();
            FridaySelectedTimes = new List<TimeOfDay>();
            SaturdaySelectedTimes = new List<TimeOfDay>();
            SundaySelectedTimes = new List<TimeOfDay>();
        }
    }
}