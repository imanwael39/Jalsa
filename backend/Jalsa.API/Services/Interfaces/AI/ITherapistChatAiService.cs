namespace Jalsa.API.Services.Interfaces.AI;

public interface ITherapistChatAiService
{
    Task<string> AnswerQuestionAsync(Guid patientId, string question, string language = "ar");
}
