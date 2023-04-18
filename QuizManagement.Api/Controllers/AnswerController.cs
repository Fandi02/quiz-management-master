using QuizManagement.Shared.Data;
using QuizManagement.Shared.Models;
using QuizManagement.Api.Models;
using QuizManagement.Api.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuizManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IScoreRepository _scoreRepository;
        private readonly IUserRepository _userRepository;

        public AnswerController(IAnswerRepository answerRepository, IQuestionRepository questionRepository, IQuizRepository quizRepository, IScoreRepository scoreRepository, IUserRepository userRepository)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _quizRepository = quizRepository;
            _scoreRepository = scoreRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets a all answers
        /// </summary>
        [OnlyAdmin]
        [HttpGet()]
        public ActionResult Answers([FromQuery] Pagination query)
        {
            return Ok(_answerRepository.GetAnswers(query));
        }

        /// <summary>
        /// Gets a specific Answer by Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAnswer(int id)
        {
            return Ok(await _answerRepository.GetAnswer(id));
        }

        /// <summary>
        /// Creates a Answer
        /// </summary>
        [OnlyAdmin]
        [HttpPost]
        public async Task<ActionResult> AddAnswer(Answer answer)
        {
            return Ok(await _answerRepository.AddAnswer(answer));
        }

        /// <summary>
        /// Updates a Answer
        /// </summary>
        [OnlyAdmin]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAnswer(int id, AnswerDTO answer)
        {
            return Ok(await _answerRepository.UpdateAnswer(new Answer
            {
                Id = id,
                Text = answer.Answer,
                Value = answer.Value,
            }));
        }

        /// <summary>
        /// Deletes a Answer with a specific Id.
        /// </summary>
        [OnlyAdmin]
        [HttpDelete]
        public async Task<ActionResult> DeleteAnswer(int id)
        {
            return Ok(await _answerRepository.DeleteAnswer(id));
        }

        /// <summary>
        /// Submit a Answer with a specific Id.
        /// </summary>
        [HttpPost("{id}/submit")]
        public async Task<ActionResult> Submit(int id)
        {
            var user = (User)HttpContext.Items["User"]!;

            if (user.Role == Role.Admin)
            {
                return Forbid();
            }

            var player = await _userRepository.GetUser(user.Id);
            var answer = await _answerRepository.GetAnswer(id);
            var question = await _questionRepository.GetQuestion(answer.QuestionId);

            await _scoreRepository.AddScore(new Score
            {
                AnswerId = answer.Id,
                QuestionId = question.Id,
                QuizId = question.QuizId,
                UserId = user.Id,
            });

            return Ok(answer);
        }
    }
}
