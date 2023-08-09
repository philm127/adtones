using EFMVC.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EFMVC.Web.ViewModels 
{
    public class CampaignProfileGeographicFormModel : ArtharFormModel
    {
        public CampaignProfileGeographicFormModel()
        {

            LocationQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Nairobi East", false}, {"Nairobi West", false}, {"Mt.Kenya", false}, {"Rift", false}, {"Western Nyanza", false}, {"Coast", false}});

   
        }

        public CampaignProfileGeographicFormModel(int CountryId)
        {
            EFMVCDataContex db = new EFMVCDataContex();

            var locationProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Location".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
            var locationProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == locationProfileMatchId).ToList();

            Dictionary<string, bool> location = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> locationlist = new List<Dictionary<string, bool>>();

            foreach (var item in locationProfileLabel)
            {
                location = new Dictionary<string, bool> { { item.ProfileLabel, false } };
                locationlist.Add(location);
            }
            LocationQuestion = CompileQuestionsDynamic(locationlist);

            //LocationQuestion =
            //    CompileQuestions(new Dictionary<string, bool>
            //                         {{"Nairobi East", false}, {"Nairobi West", false}, {"Mt.Kenya", false}, {"Rift", false}, {"Western Nyanza", false}, {"Coast", false}});

            //LocationQuestion =
            //   CompileQuestions(new Dictionary<string, bool>
            //                        {
            //                            { "Networked Youth", false}, {"Stable Hustler", false}, {"Savvy Loyalist", false},
            //                            { "Tween", false}, {"Hi-Pot students", false}, {"Prudent Young", false},
            //                            { "Young Flashers", false}, {"Mature trendies", false}, {"Settled Middle Mgmt", false},
            //                            { "Affluent Influencers", false}, {"Young cautious caller", false}, {"Toa Mpango", false},
            //                            { "Young progressive worker", false}, {"Older Toa Mpango", false}, {"Progressive worker", false},
            //                        });
        }
        // [Required]
        [RegularExpression(@"^[a-zA-Z0-9,]*$", ErrorMessage = "Only comma(,) are allowed.")]
        public string PostCode { get; set; }
       // [Required]
        public int CountryId { get; set; }
       // public string CountryName { get; set; }
        public int CampaignProfileId { get; set; }
        public int CampaignProfileGeographicId { get; set; }

        [Display(Name = "Location")]
        public List<QuestionOptionModel> LocationQuestion { get; set; }

        [Display(Name = "Location")]
        public string Location_Demographics
        {
            get
            {
                if (LocationQuestion == null)
                    LocationQuestion = new List<QuestionOptionModel>();

                return CompileAnswers(SortList(LocationQuestion));
            }
            set
            {
                if (LocationQuestion != null && LocationQuestion.Count() > 0)
                {
                    if (value == null) return;

                    for (int i = 0; i < value.Length; i++)
                        LocationQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
                }
                else
                {
                    return;
                }
            }
        }

        public bool Location { get; set; }

        //[Display(Name = "Area")]
        //public List<QuestionOptionModel> AreaQuestion { get; set; }

        //[Display(Name = "Area")]
        //public string Area_Demographics
        //{
        //    get
        //    {
        //        if (AreaQuestion == null)
        //            AreaQuestion = new List<QuestionOptionModel>();

        //        return CompileAnswers(SortList(AreaQuestion));
        //    }
        //    set
        //    {
        //        if (value == null) return;

        //        for (int i = 0; i < value.Length; i++)
        //            AreaQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
        //    }
        //}

        //public bool Area { get; set; }
    }
}