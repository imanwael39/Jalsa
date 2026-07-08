using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jalsa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedPhq9AssessmentTemplate : Migration
    {
        private const string TemplateId = "36B3C4A8-51E1-4BEC-B4A9-746B83073EE9";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
IF NOT EXISTS (SELECT 1 FROM [AssessmentTemplates] WHERE [Name] = N'phq-9')
BEGIN
    INSERT INTO [AssessmentTemplates] ([Id], [Name], [Version], [Description], [IsActive], [CreatedAt])
    VALUES ('{TemplateId}', N'phq-9', 1, N'مقياس تقييم أعراض الاكتئاب المكون من 9 بنود (Patient Health Questionnaire-9)', 1, GETUTCDATE());
END
");

            migrationBuilder.Sql($@"
IF NOT EXISTS (SELECT 1 FROM [AssessmentQuestions] WHERE [TemplateId] = '{TemplateId}')
BEGIN
    INSERT INTO [AssessmentQuestions] ([Id], [TemplateId], [QuestionText], [QuestionType], [SortOrder], [CreatedAt])
    VALUES
    (NEWID(), '{TemplateId}', N'قلة الاهتمام أو المتعة في القيام بالأشياء', N'Likert0to3', 1, GETUTCDATE()),
    (NEWID(), '{TemplateId}', N'الشعور بالحزن أو الاكتئاب أو اليأس', N'Likert0to3', 2, GETUTCDATE()),
    (NEWID(), '{TemplateId}', N'صعوبة في النوم أو البقاء نائماً، أو النوم لفترة أطول من اللازم', N'Likert0to3', 3, GETUTCDATE()),
    (NEWID(), '{TemplateId}', N'الشعور بالتعب أو قلة الطاقة', N'Likert0to3', 4, GETUTCDATE()),
    (NEWID(), '{TemplateId}', N'ضعف الشهية أو الإفراط في الأكل', N'Likert0to3', 5, GETUTCDATE()),
    (NEWID(), '{TemplateId}', N'الشعور بالسوء تجاه نفسك، أو أنك فاشل، أو أنك خذلت نفسك أو عائلتك', N'Likert0to3', 6, GETUTCDATE()),
    (NEWID(), '{TemplateId}', N'صعوبة في التركيز على أشياء مثل قراءة الجريدة أو مشاهدة التلفاز', N'Likert0to3', 7, GETUTCDATE()),
    (NEWID(), '{TemplateId}', N'التحرك أو التحدث ببطء شديد لدرجة أن الآخرين قد لاحظوا ذلك، أو العكس - الشعور بالتململ لدرجة التحرك أكثر من المعتاد', N'Likert0to3', 8, GETUTCDATE()),
    (NEWID(), '{TemplateId}', N'أفكار بأنك ستكون أفضل حالاً لو كنت ميتاً، أو أفكار لإيذاء نفسك بطريقة ما', N'Likert0to3', 9, GETUTCDATE());
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM [AssessmentQuestions] WHERE [TemplateId] = '{TemplateId}';");
            migrationBuilder.Sql($"DELETE FROM [AssessmentTemplates] WHERE [Id] = '{TemplateId}' AND NOT EXISTS (SELECT 1 FROM [Assessments] WHERE [TemplateId] = '{TemplateId}');");
        }
    }
}
