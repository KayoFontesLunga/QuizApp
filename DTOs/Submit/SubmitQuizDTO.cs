namespace QuizApp.DTOs.Submit;

public class SubmitQuizDTO
{
    public int QuizId { get; set; }
    public List<SubmittedAnswerDTO> Answers { get; set; } = [];
}
