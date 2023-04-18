using QuizManagement.Shared.Models;

namespace QuizManagement.Shared.Data
{
    public class QuestionCSV
    {
        public string Question { get; set; } = default!;

        public string Answers { get; set; } = default!;

        public List<Answer> GetAnswer()
        {
            var answers = new List<Answer>();

            foreach (var item in Answers.Split("|"))
            {
                var answer = new AnswerCSV(item);
                answers.Add(new Answer
                {
                    Text = answer.Answer,
                    Value = answer.Value
                });
            }

            return answers;
        }
    }
}
