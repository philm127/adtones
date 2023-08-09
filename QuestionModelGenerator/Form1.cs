using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestionModelGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            string[] split = inputValue.Text.Split(",".ToCharArray());

            StringBuilder builder = new StringBuilder();

            foreach (string s in split)
            {
                builder.AppendLine(@"[Display(Name = '" + s + "')]");
                builder.AppendLine(@"public List<QuestionOptionModel> " + s + "Question { get; set; }");

                builder.AppendLine(@"[Display(Name = '" + s + "')]");
                builder.AppendLine(@"public string " + s + "");
                builder.AppendLine(@"{");
                builder.AppendLine(@"get");
                builder.AppendLine(@"{");
                builder.AppendLine(@"if (" + s + "Question == null)");
                builder.AppendLine(@"" + s + "Question = new List<QuestionOptionModel>();");

                builder.AppendLine(@"return CompileAnswers(SortList(" + s + "Question));");
                builder.AppendLine(@"}");
                builder.AppendLine(@"set");
                builder.AppendLine(@"{");

                builder.AppendLine(@"" + s + "Question = CompileQuestions();");

                builder.AppendLine(@"if (value == null) return;");

                builder.AppendLine(@"for (int i = 0; i < value.Length; i++)");
                builder.AppendLine(@"" + s + "Question.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;");
                builder.AppendLine(@"}");
                builder.AppendLine(@"}");

                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine();
            }

            builder.Replace("'", @"""");

            outputValue.Text = builder.ToString();
        }

        private void generateCSHTML_Click(object sender, EventArgs e)
        {
            string[] split = inputValue.Text.Split(",".ToCharArray());

            StringBuilder builder = new StringBuilder();

            foreach (string s in split)
            {
                builder.AppendLine();

                builder.AppendLine("<div class='row'>");
                builder.AppendLine("<div class='large-6 columns'>");
                builder.AppendLine("<div class='panel'>");
                builder.AppendLine("<div class='editor-label'>");
                builder.AppendLine("@Html.LabelFor(model => model." + s + "Question)");
                builder.AppendLine("</div>");
                builder.AppendLine("<div class='clr10'></div>");
                builder.AppendLine("<div class='editor-field'>");
                builder.AppendLine("@Html.EditorFor(model => model." + s + "Question)");
                builder.AppendLine("</div>");
                builder.AppendLine("</div>");
                builder.AppendLine("</div>");
                builder.AppendLine("</div>");

                builder.AppendLine();
            }

            builder.Replace("'", @"""");

            outputValue.Text = builder.ToString();
        }
    }
}
