using QuizManagement.Shared.Data;
using QuizManagement.Shared.Models;
using QuizManagement.Api.Models;
using QuizManagement.Api.Helpers;
using QuizManagement.Api.Authorization;
using Microsoft.AspNetCore.Mvc;
using CsvHelper.Configuration.Attributes;
using CsvHelper.Delegates;
using CsvHelper.Expressions;
using CsvHelper.TypeConversion;

namespace QuizManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICSVService _csvService;

        public QuestionController(IQuestionRepository questionRepository, ICSVService csvService)
        {
            _questionRepository = questionRepository;
            _csvService = csvService;
        }

        /// <summary>
        /// Gets a all questions
        /// </summary>
        [HttpGet()]
        public ActionResult Questions([FromQuery] Pagination query)
        {
            return Ok(_questionRepository.GetQuestions(query));
        }

        /// <summary>
        /// Gets a specific Question by Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetQuestion(int id)
        {
            return Ok(await _questionRepository.GetQuestion(id));
        }

        /// <summary>
        /// Creates a Question
        /// </summary>
        [OnlyAdmin]
        [HttpPost]
        public async Task<ActionResult> AddQuestion(QuestionDTO data)
        {
            List<Answer> answers = new List<Answer>();

            if (data.Answers != null)
            {
                foreach (var answer in data.Answers)
                {
                    answers.Add(new Answer
                    {
                        Text = answer.Answer,
                        Value = answer.Value
                    });
                }
            }

            return Ok(await _questionRepository.AddQuestion(new Question
            {
                Text = data.Question,
                Answers = answers,
                QuizId = (int)data.QuizId!,
            }));
        }

        /// <summary>
        /// Get CSV Example
        /// </summary>
        [AllowAnonymous]
        [HttpGet("csv")]
        public ActionResult WriteCSV([FromForm] IFormFileCollection file)
        {
            var answers = new List<string>();
            new List<AnswerCSV>{
                new AnswerCSV{
                    Answer = "Benar",
                    Value = true
                },
                new AnswerCSV{
                    Answer = "Salah",
                    Value = false
                },
            }.ForEach(a => answers.Add(a.ToString()));

            var questions = new List<QuestionCSV>{
                new QuestionCSV{
                    Question = "Apakah benar?",
                    Answers = String.Join("|", answers)
                },
                new QuestionCSV{
                    Question = "Apakah salah?",
                    Answers = String.Join("|", answers)
                },
            };

            // _csvService.WriteCSV<QuestionCSV>(questions);

            return Ok();
        }

        /// <summary>
        /// Updates a Question with a specific ID
        /// </summary>
        [OnlyAdmin]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuestion(int id, QuestionDTO data)
        {
            Console.WriteLine($"question = {data.Question}");
            List<Answer> answers = new List<Answer>();

            if (data.Answers != null)
            {
                foreach (var answer in data.Answers)
                {
                    answers.Add(new Answer
                    {
                        Id = answer.Id == null ? default(int) : (int)answer.Id,
                        Text = answer.Answer,
                        Value = answer.Value
                    });
                }
            }
            Console.WriteLine($"question = {data.Question}");


            // return Ok();

            return Ok(await _questionRepository.UpdateQuestion(new Question
            {
                Id = id,
                Text = data.Question,
                Answers = answers,
                QuizId = data.QuizId == null ? default(int) : (int)data.QuizId,
            }));
        }

        /// <summary>
        /// Deletes a Question with a specific Id.
        /// </summary>
        [OnlyAdmin]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            return Ok(await _questionRepository.DeleteQuestion(id));
        }
    }
}
