using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuizManagement.Shared.Data
{
    public class AnswerCSV
    {
        public string Answer { get; set; } = default!;
        public bool Value { get; set; }

        // class to string
        public override string ToString()
        {
            return Answer + "," + Value.ToString();
        }

        public AnswerCSV() { }

        // string to class
        public AnswerCSV(string myclassTostring)
        {
            string[] props = myclassTostring.Split(',');
            Answer = props[0];
            Value = Convert.ToBoolean(props[1]);
        }
    }
}
