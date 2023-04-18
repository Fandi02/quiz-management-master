using QuizManagement.Shared.Data;
using QuizManagement.Shared.Models;
using QuizManagement.Api.Models;
using QuizManagement.Api.Helpers;
using QuizManagement.Api.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuizManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IScoreRepository _scoreRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ICSVService _csvService;
        private readonly IUserRepository _userRepository;

        public QuizController(IQuizRepository quizRepository, IQuestionRepository questionRepository, IScoreRepository scoreRepository, IUserRepository userRepository, ICSVService csvService)
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _scoreRepository = scoreRepository;
            _userRepository = userRepository;
            _csvService = csvService;
        }

        /// <summary>
        /// Gets a all quizzes
        /// </summary>
        [OnlyAdmin]
        [HttpGet()]
        public async Task<ActionResult> Quizzes()
        {
            return Ok(await _quizRepository.GetQuizzes());
        }

        /// <summary>
        /// Gets a specific Quiz by Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetQuiz(int id)
        {
            return Ok(await _quizRepository.GetQuiz(id));
        }

        /// <summary>
        /// Creates a Quiz
        /// </summary>
        [OnlyAdmin]
        [HttpPost]
        public async Task<ActionResult> AddQuiz(QuizDTO data)
        {
            var user = (User)HttpContext.Items["User"]!;

            List<Question> questions = new List<Question>();

            return Ok(await _quizRepository.AddQuiz(new Quiz
            {
                Name = data.Name,
                Questions = questions,
            }));
        }

        /// <summary>
        /// Updates a Quiz with a specific ID
        /// </summary>
        [OnlyAdmin]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuiz(int id, QuizDTO data)
        {
            return Ok(await _quizRepository.UpdateQuiz(new Quiz
            {
                Id = id,
                Name = data.Name,
            }));
        }

        /// <summary>
        /// Deletes a Quiz with a specific Id.
        /// </summary>
        [OnlyAdmin]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuiz(int id)
        {
            return Ok(await _quizRepository.DeleteQuiz(id));
        }

        /// <summary>
        /// Get Leaderboard
        /// </summary>
        [HttpGet("{id}/leaderboard")]
        public async Task<ActionResult> GetLeaderboard(int id)
        {
            return Ok(await _scoreRepository.GetLeaderboard(id));
        }

        /// <summary>
        /// Get Leaderboard
        /// </summary>
        [HttpGet("{id}/player")]
        public async Task<ActionResult> GetPlayers(int id)
        {
            return Ok(await _userRepository.GetByQuiz(id));
        }

        /// <summary>
        /// Get CSV
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}/player/csv")]
        public async Task<ActionResult> GetPlayersCSV(int id)
        {
            var players = await _userRepository.GetByQuiz(id);
            var file = _csvService.WriteCSV<User>(players);

            Response.Headers["Content-Disposition"] = "attachment; filename=players.csv";
            Response.Headers.Add("Content-Type", "application/csv");

            return File(file, "text/csv");
        }

        /// <summary>
        /// Upload Questions
        /// </summary>
        [HttpPost("{id}/upload")]
        public async Task<ActionResult> UploadQuestion(int id, [FromForm] IFormFileCollection file)
        {
            var csv = _csvService.ReadCSV<QuestionCSV>(file[0].OpenReadStream());
            var questions = new List<Question>();

            foreach (var item in csv)
            {
                var answers = item.GetAnswer();

                await _questionRepository.AddQuestion(new Question
                {
                    Text = item.Question,
                    Answers = answers,
                    QuizId = id,
                });
            }

            return Ok();
        }
    }
}

public class FileDTO
    {
        public IFormFile Filesawd { get; set; } = default!;
    }
