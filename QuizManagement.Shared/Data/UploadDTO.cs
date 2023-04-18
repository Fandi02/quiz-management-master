using System.ComponentModel.DataAnnotations;

namespace QuizManagement.Shared.Data
{
    public class UploadDTO
    {
        [Required]
        public Stream File { get; set; } = default!;
    }
}
